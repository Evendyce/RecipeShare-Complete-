# 📖 RecipeShare — Take-Home Assessment

Welcome to **RecipeShare**, a vibrant platform where home cooks and food bloggers can create, view, and share recipes in a beautifully styled, user-friendly space. This app was built as a take-home assessment to demonstrate real-world development skills across architecture, design, and feature implementation.

---

## 🛠️ Tech Stack

- [x] **Blazor Server (.NET 8)**
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
| 📤 Export to Markdown + PDF                  | ⏳ Planned     |
| 🧪 Unit Tests (xUnit)                        | ⏳ Planned     |
| 🕒 500x GET Benchmark Test                   | ⏳ Planned     |
| 🔧 Dockerfile                                | ⏳ Planned     |
| 📄 SOLUTION.md                               | ⏳ Planned     |
| 📹 Loom Demo Video                           | ⏳ Planned     |

---

## 🌐 Navigation & Pages

| Page                         | Status    |
| ---------------------------- | --------- |
| 🧭 Top NavBar (Login/Profile) | ✅ Planned |
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
| Dockerfile                | ⏳ Planned     |
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
| Animated Card/Grid system         | ⏳ Planned  |
| Attribution license block         | ✅ Added    |

> Attribution required for use.  
> See `LICENSE.txt` for usage rights.
