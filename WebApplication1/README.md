# ⚙️ .NET Authenticator — Projet de Back-Office pour Interventions

![dev happy](https://i.giphy.com/YQitE4YNQNahy.webp)

Bienvenue dans **le projet d'API back-office le plus stylé de l'année**. Cette app .NET 8 gère des interventions techniques à domicile avec :
- des clients 👨‍💼
- des techniciens 🧰
- des admins puissants 👑

## ✨ Fonctionnalités

- 🔐 Authentification **JWT + ASP.NET Identity**
- 🔁 Refresh tokens ultra sécurisés
- 🧠 Architecture en couches **(Controller → Service → DataAccess)**
- 🌍 Localisation multilingue (🇫🇷 & 🇬🇧)
- 👮 Rôles `admin` / `technician` gérés au poil
- 🧱 Base de données SQLite + EF Core + Seed automatique

## 🧠 Architecture

├── Controllers/       # Entrées API

├── Services/          # Logique métier

├── DataAccess/        # Accès base (EF Core)

├── Data/Entity/       # Entités persistées

├── Program.cs         # Bootstrap de l’app

├── appsettings.json   # Configurations

---

## 🚀 Démarrage rapide

### 1. Clone le projet

```bash
git clone https://github.com/ronanhenry-web/.NET-Authenticator.git
cd .NET-Authenticator
```

### 2. Configure SQLite

Modifie `appsettings.json` :

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=articles.db"
}
```

### 3. Mets à jour la base de données

```bash
dotnet ef database update
```

### 4. Lance l’appli

```bash
dotnet run
```

Accès API : [http://localhost:5000/swagger](http://localhost:5000/swagger)  
(ou https si activé)

![Code Work's](https://media1.giphy.com/media/v1.Y2lkPTc5MGI3NjExdzN3ZWsxeG84c2F6Y2ZqOW5vbHhscjloYWFsdWM4b29uajZxMTVvYSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/12BYUePgtn7sis/giphy.gif)

---

## 🔑 Endpoints Authentification

| Méthode | Route                  | Description                       |
|--------|------------------------|-----------------------------------|
| POST   | `/api/auth/register`   | ✅ Créer un utilisateur (admin)   |
| POST   | `/api/auth/login`      | 🔐 Login + génération de JWT      |
| POST   | `/api/auth/refresh`    | 🔄 Rafraîchir un access token     |

---

## 📰 Endpoints Articles

| Méthode | Route                | Description                  |
|--------|----------------------|------------------------------|
| GET    | `/api/articles`      | Voir tous les articles       |
| GET    | `/api/articles/{id}` | Détail d’un article          |
| POST   | `/api/articles`      | Créer un article             |
| DELETE | `/api/articles/{id}` | Supprimer un article         |

---

## 🔧 Endpoints Interventions

| Méthode | Route                  | Rôle requis | Description                               |
|--------|------------------------|-------------|-------------------------------------------|
| GET    | `/api/intervention`    | tous        | Technicien : ses interventions<br>Admin : toutes |
| POST   | `/api/intervention`    | admin       | Créer une nouvelle intervention           |

> ✨ Interventions = Client + ServiceType + Techniciens + Matériaux

---

## 🔒 Endpoints Admin

| Méthode | Route              | Description                        |
|--------|--------------------|------------------------------------|
| GET    | `/admin/crash`     | 💥 Réponse humoristique “Boom”     |
| GET    | `/admin/message`   | 🧨 Déclenche une exception custom  |

---

## 🧪 Tester avec Swagger

Swagger est préconfiguré pour :
- Utiliser ton token **sans préfixe "Bearer "**  
- Tester en live les routes avec différents rôles

Tu peux simplement entrer le token brut dans Swagger, et il le préfixera pour toi :

```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 🛡️ Sécurité

- 🔒 Passwords hachés avec PBKDF2
- ✅ Validation automatique des claims (email, roles…)
- 🔁 Refresh Tokens liés à l’utilisateur et validés
- 🧼 Gestion des erreurs au format [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807)

---

## 🤓 Exemple de login avec Postman

```json
POST /api/auth/login
{
  "email": "admin@admin.com",
  "password": "Admin123!"
}
```

---

## 📚 Bonus & Ressources

Inspiré de tutoriels .NET avancés :
- Clean Architecture
- Gestion des rôles
- JWT dans la vraie vie
- Gestion multilingue
- Migrations / Seed / Identity

---

## 🎉 Contribuer

Ce projet est **fun, complet, et flexible**. Tu peux :
- le cloner 🧬
- le forker 🍴
- l'améliorer 🚀

![dotnet fun](https://i.giphy.com/Qn74oPyaKYBpVWdA7t.webp)

---

**Fait avec ❤️ en .NET**
