using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ShNotes.Caching.InMemory.Notes;
using ShNotes.UseCases.Notes;
using ShNotes.UseCases.Notes.AddNote;
using ShNotes.UseCases.Notes.ChangeNoteName;
using ShNotes.UseCases.Notes.ChangeNoteStatus;
using ShNotes.UseCases.Notes.GetNote;
using ShNotes.UseCases.Notes.GetNotes;
using ShNotes.UseCases.Notes.RemoveNote;

namespace ShNotes.Caching;

public static class DependencyInjection
{
    public static IServiceCollection AddCaching(this IServiceCollection services)
    {
        services.AddMemoryCache();

        services.AddMediatR(c =>
            c.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
        );

        services.AddScoped<GetNoteQueryHandler>();
        services.AddScoped<CachedGetNoteQueryHandler>();
        services.AddScoped<IRequestHandler<GetNoteQuery, NoteDto>>(sp =>
            sp.GetRequiredService<CachedGetNoteQueryHandler>()
        );

        services.AddScoped<GetNotesQueryHandler>();
        services.AddScoped<CachedGetNotesQueryHandler>();
        services.AddScoped<IRequestHandler<GetNotesQuery, IEnumerable<ShortNoteDto>>>(sp =>
            sp.GetRequiredService<CachedGetNotesQueryHandler>()
        );

        services.AddScoped<AddNoteCommandHandler>();
        services.AddScoped<CachedAddNoteCommandHandler>();
        services.AddScoped<IRequestHandler<AddNoteCommand, int>>(sp =>
            sp.GetRequiredService<CachedAddNoteCommandHandler>()
        );

        services.AddScoped<AddNoteCommandHandler>();
        services.AddScoped<CachedAddNoteCommandHandler>();
        services.AddScoped<IRequestHandler<AddNoteCommand, int>>(sp =>
            sp.GetRequiredService<CachedAddNoteCommandHandler>()
        );

        services.AddScoped<ChangeNoteNameCommandHandler>();
        services.AddScoped<CachedChangeNoteNameCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeNoteNameCommand, NoteDto>>(sp =>
            sp.GetRequiredService<CachedChangeNoteNameCommandHandler>()
        );

        services.AddScoped<ChangeNoteDescriptionCommandHandler>();
        services.AddScoped<CachedChangeNoteDescriptionCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeNoteDescriptionCommand, NoteDto>>(sp =>
            sp.GetRequiredService<CachedChangeNoteDescriptionCommandHandler>()
        );

        services.AddScoped<ChangeNoteStatusCommandHandler>();
        services.AddScoped<CachedChangeNoteStatusCommandHandler>();
        services.AddScoped<IRequestHandler<ChangeNoteStatusCommand>>(sp =>
            sp.GetRequiredService<CachedChangeNoteStatusCommandHandler>()
        );

        services.AddScoped<RemoveNoteCommandHandler>();
        services.AddScoped<CachedRemoveNoteCommandHandler>();
        services.AddScoped<IRequestHandler<RemoveNoteCommand>>(sp =>
            sp.GetRequiredService<CachedRemoveNoteCommandHandler>()
        );
        return services;
    }
}
