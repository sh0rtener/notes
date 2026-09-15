FROM node:20-alpine AS build
WORKDIR /app
COPY client/web/package*.json ./
RUN npm ci
COPY client/web/ ./
RUN npm run build

FROM node:20-alpine AS runtime
WORKDIR /app
COPY --from=build /app/package*.json ./
COPY --from=build /app/dist ./dist
RUN npm ci --omit=dev

ENV NODE_ENV=production
EXPOSE 4000

CMD ["node", "dist/web/server/server.mjs"]