# 📖 RecipeShare — Take-Home Assessment

Welcome to **RecipeShare**, a vibrant platform where home cooks and food bloggers can create, view, and share recipes in a beautifully styled, user-friendly space. This app was built as a take-home assessment to demonstrate real-world development skills across architecture, design, and feature implementation.

---

## 🛠️ Tech Stack

* [x] **Blazor Web App (.NET 9)**
* [x] **ASP.NET Core Web API (.NET 9)**
* [x] **Entity Framework Core**
* [x] **SQL Server Express**
* [x] **ASP.NET Core Identity (Username-based)**
* [x] **EF Core Power Tools** (Reverse-Engineered Models)
* [x] **VoidGlass Theme** (Custom neon-glass UI)
* [x] **Font Awesome (local)**

---

<details>
<summary>🧱 Architecture</summary>

| Task                                                   | Status     |
| ------------------------------------------------------ | ---------- |
| Dual DbContexts: Identity & App                        | ✅ Complete |
| DbContextFactory for app context                       | ✅ Complete |
| Clean DI registration                                  | ✅ Complete |
| Auto-profile generation on register                    | ✅ Complete |
| DB split: Recipes, Steps, Images, Favourites, Profiles | ✅ Complete |

</details>

## ✨ Features

| Feature                                      | Status     |
| -------------------------------------------- | ---------- |
| 🔐 Username-based Register/Login             | ✅ Complete |
| 👤 Auto UserProfile Creation                 | ✅ Complete |
| 🍲 Recipes CRUD                              | ✅ Complete |
| 🣟 `RecipeSteps` for structured instructions | ✅ Complete |
| 🖼️ Multiple Recipe Images                   | ✅ Complete |
| ❤️ Favourites (per-user toggle)              | ✅ Complete |
| 📄 Export to Markdown + PDF                  | ✅ Complete |
| ✅ Interactive Step Checklist                 | ✅ Complete |
| 📄 Default Data Seeding                      | ✅ Complete |
| 🖋️ Smart Tag + Step + Image Syncing         | ✅ Complete |
| 𞢪 Unit Tests (xUnit)                        | ✅ Complete |
| 🥒 500x GET Benchmark Test                   | ✅ Complete |
| 🔧 Dockerfile                                | ✅ Complete |
| 📄 SOLUTION.md                               | ✅ Complete |
| 📹 Loom Demo Video                           | ✅ Complete |

---

## 🔧 API Capabilities

<details>
<summary>Expand to view API capabilities</summary>

| Capability                         | Status     |
| ---------------------------------- | ---------- |
| RESTful Recipe Endpoints (CRUD)    | ✅ Complete |
| Shared DTOs & ViewModel Separation | ✅ Complete |
| Search + Filter DTOs               | ✅ Complete |
| Skip-Navigation Tag Mapping        | ✅ Complete |
| Manual Custom Mapper System        | ✅ Complete |

</details>

---

## 🔍 Filtering & Search

<details>
<summary>Expand to view supported filters</summary>

| Filter Type              | Status     |
| ------------------------ | ---------- |
| By Tag (single/multiple) | ✅ Complete |
| By Title (partial match) | ✅ Complete |
| By Ingredient            | ✅ Complete |
| By Cooking Time Range    | ✅ Complete |
| Tile vs Full View Toggle | ✅ Complete |

</details>

---

<details>
<summary>🌐 Navigation & Pages</summary>

| Page                          | Status     |
| ----------------------------- | ---------- |
| 🏍 Top NavBar (Login/Profile) | ✅ Complete |
| 🔍 Global Recipes View        | ✅ Complete |
| 👤 My Recipes View            | ✅ Complete |
| ➕ Add/Edit Recipe             | ✅ Complete |
| 📄 Recipe Detail View         | ✅ Complete |
| ❤️ My Favourites              | ✅ Complete |

</details>

---

<details>
<summary>📂 Database Schema</summary>

| Table              | Status     |
| ------------------ | ---------- |
| `Recipes`          | ✅ Complete |
| `RecipeSteps`      | ✅ Complete |
| `RecipeImages`     | ✅ Complete |
| `RecipeFavourites` | ✅ Complete |
| `UserProfiles`     | ✅ Complete |
| `Tags`             | ✅ Complete |
| `RecipeTags`       | ✅ Complete |

> ✅ `Steps` string field remains for spec compliance, synced from structured steps.
> 🔗 `RecipeTags` is an explicitly defined many-to-many join table between `Recipes` and `Tags`, used for dietary filtering and metrics.

</details>

---

<details>
<summary>📸 Image Handling</summary>

| Feature                            | Status     |
| ---------------------------------- | ---------- |
| File uploads to `/wwwroot/uploads` | ✅ Complete |
| Temp upload staging (GUID folder)  | ✅ Complete |
| Server-side move + path rewrite    | ✅ Complete |
| DB path storage in `RecipeImages`  | ✅ Complete |
| Cover image support                | ✅ Complete |
| Display order & captions           | ✅ Complete |

</details>

---

<details>
<summary>📊 Benchmark, 🧢 Testing & 📦 Deployment</summary>

#### 📊 Benchmark

| Metric                        | Status     |
| ----------------------------- | ---------- |
| 500x GET `/recipes` (Release) | ✅ Complete |
| Output to `README.md`         | ✅ Included |

#### 🧢 Testing

| Area              | Status     |
| ----------------- | ---------- |
| Recipe CRUD tests | ✅ Complete |

#### 📦 Deployment & Docs

| Item                      | Status        |
| ------------------------- | ------------- |
| Dockerfile                | ✅ Complete    |
| GitHub Actions (optional) | ❌ Not planned |
| `README.md` (this!)       | ✅ Complete |
| `SOLUTION.md`             | ✅ Complete    |
| Loom Walkthrough Video    | ✅ Complete |

</details>

---

## 📈 Benchmark Results

Executed `GET /api/recipes` 500 times in Release mode against local SQL Server.

| Metric               | Value                            |
| -------------------- | -------------------------------- |
| Average Latency (ms) | 9.20 ms                          |
| Total Duration       | \~4.60 sec                       |
| Environment          | .NET 9.0, Localhost, SQL Express |

> Measured using `Stopwatch` in a dedicated xUnit test.

---

## 🎨 VoidGlass Theme

| Item                              | Status     |
| --------------------------------- | ---------- |
| SCSS/CSS included                 | ✅ Complete |
| Footer / Header custom components | ✅ Complete |
| Theme Switcher Toggle             | ✅ Complete |
| Animated Card/Grid system         | ✅ Complete |
| FilterBar Styling & Sticky Logic  | ✅ Complete |
| Favourite Toggle with Icons       | ✅ Complete |
| Attribution license block         | ✅ Added    |

> Attribution required for use.
> See `LICENSE.txt` for usage rights.

---

## 🚀 Getting Started

Clone the repo, open `RecipeShare.sln`, and build/run from Visual Studio.

Ensure SQL Server Express is available and the connection string is correct inside `appsettings.json`.

---

## 🧵 Test Execution Notes

To ensure integration tests pass, the database **must be seeded** before running tests.

### ✅ Seed Before Running Tests

Before executing:

```bash
dotnet test
```

Make sure you:

1. Launch the app at least once and click the **"Seed Demo Data"** button on the homepage
   **OR**
2. Manually trigger the seeding endpoint (if exposed)
3. Ensure the `DemoUser` and required lookup data exist in the database

Tests rely on:

* `DemoUser` being present
* Recipe creation logic having a valid `UserId` to assign
* Tag and image-related joins being prepped

> ⚠️ Tests will **fail or behave unexpectedly** if the seeded data is missing.

---

## 📦 Database Seeding

To seed the database with a demo user and sample recipes, click the **"Seed Demo Data"** button on the homepage after launching the app.

<details>
<summary>🔐 Demo Login Credentials (Seeded)</summary>

* **Username:** `DemoUser`
* **Password:** `Password123!`

> This account is seeded automatically and assigned the `User` role.
> Perfect for testing recipe features and profile display.

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

The **RecipeShare API** is containerized using Docker for demonstration purposes.

### 🛠️ Build & Run:

```bash
docker build -t recipeshare-api -f ./RecipeShare.API/Dockerfile .
docker run -p 5000:80 recipeshare-api
```

Once running, access the Swagger UI at:

```
http://localhost:5000/swagger
```

### 📌 Notes:

* The container exposes the API on internal port **80**, mapped to **localhost:5000**.
* Only the **API project** is containerized.
* The **Blazor Server frontend** runs outside Docker to avoid complications with:

  * Local database access (`host.docker.internal` issues, `sa` config, trust certs, etc.)
  * NuGet fallback path bugs inside SDK containers.
* For a smoother experience, run the API using **Visual Studio** or `dotnet run` during development.

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

---

## 🛋️ Runtime Notes

* Static image uploads are served via `/uploads` mapped in `Program.cs`:

  ```csharp
  app.UseStaticFiles(new StaticFileOptions
  {
      FileProvider = new PhysicalFileProvider(
          Path.Combine(env.WebRootPath, "uploads")),
      RequestPath = "/uploads"
  });
  ```
* Images are uploaded to:
  `/wwwroot/uploads/RecipeImages/{recipeId}/[CoverImage|Additional]/`
* New recipes use a temporary staging folder:
  `/wwwroot/uploads/TempRecipeImages/{guid}/...`, which is finalized post-creation.

---

> Tip: Use the new `.vg-panel` class to wrap entire form/card sections with VoidGlass style.
