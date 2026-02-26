# us-notification-api — Contexte projet

## Description
API ASP.NET Core (.NET 8) multi-usage, projet d'apprentissage de l'architecture hexagonale / clean architecture.
Projet renommé à terme : `UrgenceSante.NotificationApi` (actuellement `PocMissionPush`).

## Objectifs pédagogiques
- Clean Architecture / Architecture Hexagonale (Ports & Adapters)
- Design Patterns : expliquer AVANT d'appliquer si c'est la première fois
- Principes SOLID, KISS, Clean Code
- Expliquer chaque modification faite

## Fonctionnalités
1. **Notifications Web Push** (VAPID) — fonctionnalité principale à finir
2. **Subscriptions** — gestion des abonnements push
3. **Loans** — calculs d'emprunts (amortissement, capital restant, dashboard)
4. **Keycloak** — gestion utilisateurs via API Admin Keycloak
5. **WorkSession** — sessions de travail (stub/mock pour l'instant)

## Stack technique
- .NET 8, ASP.NET Core Web API
- Entity Framework Core 9 (InMemory + SQL Server)
- WebPush (VAPID notifications)
- CsvHelper (import CSV des emprunts)
- Keycloak (auth JWT Bearer)
- Swagger / Swashbuckle

## Problèmes identifiés (à corriger)
- [ ] Namespace incohérent : `PocMissionPush` partout
- [ ] Structure de dossiers chaotique : fichiers dupliqués, dossiers en doublon
  - `Modules/Subscriptions/` ET `Models/Subscription/` pour les mêmes entités
  - `Api/Controllers/` ET `Modules/*/` pour les controllers
  - `Controllers/SubscriptionCmd.cs` perdu à la racine
- [ ] `WeatherForecast.cs` et `WeatherForecastController.cs` — résidus du template
- [ ] `Application/Subscription/CreateSubscription.cs` — fichier vide
- [ ] Champs publics dans les controllers (`public LoanService _loanService`)
- [ ] `UseAuthorization()` appelé deux fois dans `Program.cs`
- [ ] Secrets Keycloak hardcodés dans `KeycloakAdminService.cs` ET dans `appsettings.json`
- [ ] URLs Keycloak hardcodées dans le code au lieu de la config
- [ ] Pas de gestion d'erreurs cohérente (Result pattern)
- [ ] Méthodes non implémentées (`throw new NotImplementedException()`)

## Architecture cible (Hexagonale / Clean)
```
UsNotificationApi/
├── Domain/                    # Entités pures, interfaces de ports
│   ├── Subscriptions/
│   ├── Notifications/
│   ├── Loans/
│   ├── Users/
│   └── WorkSessions/
├── Application/               # Cas d'usage (Use Cases), DTOs
│   ├── Subscriptions/
│   ├── Notifications/
│   ├── Loans/
│   └── Users/
├── Infrastructure/            # Adaptateurs sortants (DB, APIs externes)
│   ├── Persistence/
│   │   ├── Contexts/
│   │   └── Repositories/
│   └── Keycloak/
└── Api/                       # Adaptateur entrant (Controllers, DTOs HTTP)
    └── Controllers/
```

## Ordre de refactoring prévu
1. Nettoyer les fichiers parasites (WeatherForecast, doublons, vides)
2. Renommer le namespace
3. Réorganiser la structure selon l'archi cible
4. Appliquer les principes SOLID un par un
5. Finir la fonctionnalité Notifications/Registration

## Fichiers clés
- `Program.cs` — composition root / DI
- `PocMissionPush.csproj` — à renommer
- `Modules/Notifications/PushService.cs` — service principal push
- `Infrastructure/Repositories/SubscriptionRepository.cs` — repo abonnements
- `Modules/KeycloakAdmin/KeycloakAdminService.cs` — ATTENTION secrets hardcodés
- `appsettings.json` — ATTENTION client_secret en clair

## Préférences workflow
- Expliquer chaque design pattern/principe la première fois qu'on l'applique
- Expliquer chaque modification effectuée
- Approche progressive : une chose à la fois, ne pas tout casser d'un coup
