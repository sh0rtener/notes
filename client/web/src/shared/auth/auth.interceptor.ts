import { HttpErrorResponse, HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { AuthService } from "./auth.service";
import { catchError, switchMap, throwError } from "rxjs";
import { Router } from "@angular/router";

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    const token = authService.getToken();

    const request = token ? req.clone({
        setHeaders: {
            Authorization: `Bearer ${token}`
        }
    }) : req;

    return next(request).pipe(
        catchError((e: HttpErrorResponse) => {
            const refreshToken = authService.getRefreshToken();

            if (!refreshToken || !authService.canRefresh()) {
                authService.logout();
                router.navigate(['/'])

                return throwError(() => e);
            }

            return authService.refresh(refreshToken).pipe(
                switchMap(() => {
                    const newToken = authService.getToken();

                    if (!newToken) {
                        authService.logout();
                        router.navigate(['/'])

                        return throwError(() => e);
                    }

                    const retryReq = req.clone({
                        setHeaders: {
                            Authorization: `Bearer ${newToken}`
                        }
                    });

                    return next(retryReq);
                }),
                catchError(refreshError => {
                    authService.logout();
                    router.navigate(['/'])

                    return throwError(() => refreshError);
                })
            );
        })
    )
}