# 📖 RecipeShare — Take-Home Assessment

Welcome to **RecipeShare**, a vibrant platform where home cooks and food bloggers can create, view, and share recipes in a beautifully styled, user-friendly space. This app was built as a take-home assessment to demonstrate real-world development skills across architecture, design, and feature implementation.

---

## 🛠️ Tech Stack

- [x] **Blazor Web App (.NET 9)**
- [x] **ASP.NET Core Web API (.NET 9)**
- [x] **Entity Framework Core**
- [x] **SQL Server Express**
- [x] **ASP.NET Core Identity (Username-based)**
- [x] **EF Core Power Tools** (Reverse-Engineered Models)
- [x] **VoidGlass Theme** (Custom neon-glass UI)
- [x] **Font Awesome (local)**

---

## 🧱 Architecture

| Task                                                   | Status     |
| ------------------------------------------------------ | ---------- |
| Dual DbContexts: Identity & App                        | ✅ Complete |
| DbContextFactory for app context                       | ✅ Complete |
| Clean DI registration                                  | ✅ Complete |
| Auto-profile generation on register                    | ✅ Complete |
| DB split: Recipes, Steps, Images, Favourites, Profiles | ✅ Complete |

---

## ✨ Features

| Feature                                     | Status        |
| ------------------------------------------- | ------------- |
| 🔐 Username-based Register/Login             | ✅ Complete    |
| 👤 Auto UserProfile Creation                 | ✅ Complete    |
| 🍲 Recipes CRUD                              | ✅ In Progress |
| 🧾 `RecipeSteps` for structured instructions | ✅ Complete    |
| 🖼️ Multiple Recipe Images                    | ✅ Complete    |
| ❤️ Favourites (per-user toggle)              | ✅ Planned     |
| 🔍 Dietary Tag Filtering                     | ✅ Planned     |
| 📄 Export to Markdown + PDF                  | ⏳ Planned     |
| 📄 Default Data Seeding						| ✅ Complete     |
| 🧪 Unit Tests (xUnit)                        | ⏳ Planned     |
| 🥒 500x GET Benchmark Test                   | ⏳ Planned     |
| 🔧 Dockerfile                                | ✅ Complete    |
| 📄 SOLUTION.md                               | ⏳ Planned     |
| 📹 Loom Demo Video                           | ⏳ Planned     |

---

## 🌐 Navigation & Pages

| Page                         | Status    |
| ---------------------------- | --------- |
| 🛍 Top NavBar (Login/Profile) | ✅ Complete |
| 🔍 Global Recipes View        | ⏳ Planned |
| 👤 My Recipes View            | ⏳ Planned |
| ➕ Add/Edit Recipe            | ⏳ Planned |
| 📄 Recipe Detail View         | ⏳ Planned |
| ❤️ My Favourites              | ⏳ Planned |

---

## 📂 Database Schema

| Table              | Status     |
| ------------------ | ---------- |
| `Recipes`          | ✅ Complete |
| `RecipeSteps`      | ✅ Complete |
| `RecipeImages`     | ✅ Complete |
| `RecipeFavourites` | ✅ Complete |
| `UserProfiles`     | ✅ Complete |

> ✅ `Steps` string field remains for spec compliance, synced from structured steps.

---

## 📸 Image Handling

| Feature                            | Status     |
| ---------------------------------- | ---------- |
| File uploads to `/wwwroot/uploads` | ✅ Planned  |
| DB path storage in `RecipeImages`  | ✅ Complete |
| Cover image support                | ✅ Complete |
| Display order & captions           | ✅ Complete |

---

## 📊 Benchmark

| Metric                        | Status    |
| ----------------------------- | --------- |
| 500x GET `/recipes` (Release) | ⏳ Planned |
| Output to `README.md`         | ⏳ Planned |

---

## 🧪 Testing

| Area                   | Status    |
| ---------------------- | --------- |
| Recipe CRUD tests      | ⏳ Planned |
| Favourite toggle tests | ⏳ Planned |
| Profile creation tests | ⏳ Planned |

---

## 📦 Deployment & Docs

| Item                      | Status        |
| ------------------------- | ------------- |
| Dockerfile                | ✅ Complete    |
| GitHub Actions (optional) | ❌ Not planned |
| `README.md` (this!)       | ✅ In Progress |
| `SOLUTION.md`             | ⏳ Planned     |
| Loom Walkthrough Video    | ⏳ Planned     |

---

## 🎨 VoidGlass Theme

| Item                              | Status     |
| --------------------------------- | ---------- |
| SCSS/CSS included                 | ✅ Complete |
| Footer / Header custom components | ✅ Complete |
| Theme Switcher Toggle				| ✅ Complete |
| Animated Card/Grid system         | ⏳ Planned  |
| Attribution license block         | ✅ Added    |

> Attribution required for use.\
> See `LICENSE.txt` for usage rights.

---

## 🚀 Getting Started

Clone the repo, open `RecipeShare.sln`, and build/run from Visual Studio.

Ensure SQL Server Express is available and the connection string is correct inside `appsettings.json`.

---

## 📦 Database Seeding

To seed the database with a demo user and sample recipes, click the **"Seed Demo Data"** button on the homepage after launching the app.

<details>
<summary>🔐 Demo Login Credentials</summary>

- **Username:** `DemoUser`  
- **Password:** `Password123!`  

> This seeded user is assigned the `User` role and owns pre-loaded recipes and profile data.
</details>

---

## 🧶 SCSS Compilation

Custom styles for the **VoidGlass Theme** are authored in SCSS and compiled to CSS for browser use.

### To edit styles:
Modify any SCSS files under:

```
RecipeShare.Web/wwwroot/voidglass/
```

### To regenerate the CSS:
You’ll need [Dart Sass](https://sass-lang.com/install). Install it via:

```bash
npm install -g sass
```

Then run this from the project root:

```bash
sass wwwroot/voidglass/voidglass.scss wwwroot/css/voidglass.css --no-source-map --style=compressed
```

> ⚠️ This step is only needed if you change SCSS styles. It's not required to build or run the app.

---

## 🐳 Docker Support

The RecipeShare API is containerized using Docker for demonstration purposes.

### Build & Run:

```bash
docker build -t recipeshare-api -f ./RecipeShare.API/Dockerfile .
docker run -p 5000:8080 recipeshare-api
```

Once running, visit:

```
http://localhost:5000/swagger
```

### Notes:

- The container exposes the API on port **8080**, mapped to **localhost:5000** for convenience.
- Only the **API project** is containerized — the Blazor Server frontend runs outside Docker to simplify the flow and avoid NuGet complications.

---

## 🔍 Swagger in Production

For ease of testing and demonstration, **Swagger UI is enabled in both Development and Production**.

This is handled in `Program.cs` as follows:

```csharp
// For realworld scenarios, uncomment the below
// if (app.Environment.IsDevelopment()) { app.MapOpenApi(); }

// Comment this out for realworld
app.MapOpenApi();
```

In real-world deployments, Swagger should be disabled or protected in production.