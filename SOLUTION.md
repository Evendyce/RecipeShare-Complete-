# 🔧 RecipeShare — SOLUTION.md

This document outlines the architectural decisions, implementation rationale, trade-offs, and personal reflections behind the development of **RecipeShare**, a take-home assessment project.

---

## 🌍 Project Goals

- Build a **recipe-sharing platform** with full CRUD capabilities.
- Enable user interaction through favourites, filtering, and step tracking.
- Apply clean, maintainable design with a **custom VoidGlass UI theme**.

---

## ✅ Spec Requirements — Implemented

| Spec Task                                                          | Status     |
| ------------------------------------------------------------------ | ---------- |
| Create ASP.NET Core 6+ project                                     | ✅ Complete |
| Define Recipe model (ID, title, ingredients, steps, cooking time, tags) | ✅ Complete |
| Implement full RESTful CRUD endpoints                              | ✅ Complete |
| Use EF Core + SQL Server                                           | ✅ Complete |
| Seed database with ≥ 3 example recipes                             | ✅ Complete |
| LINQ-based filtering (e.g. dietary tags)                           | ✅ Complete |
| Add validation (e.g. required title, cooking time > 0)             | ✅ Complete |
| Include unit tests (xUnit)                                         | ✅ Complete |
| Benchmark 500x GET /api/recipes                                    | ⏳ Planned  |
| Build front end (list, detail, add/edit/delete, validation)        | ✅ Complete |

---

## ✨ Additional Features (Beyond Spec)

| Feature                                                           | Description |
| ----------------------------------------------------------------- | ----------- |
| Structured step editing                                           | Synced to flat `Steps` string for spec compatibility |
| Per-user Favourites                                               | Toggle and counter system |
| Markdown checklist                                                | Interactive `[ ]` rendering |
| 📄 Export to PDF or Markdown                                      | Both formats supported — Markdown client-side, PDF via `html2pdf.js` |
| 🖼️ Recipe Images with Carousel                                    | Cover image + additional images with order & captions |
| 🧱 Steps and Tags in separate tables                              | Normalized design for better filtering, metrics, and editing UX |
| Tile/grid responsive layout                                       | With tag badges and filters |
| Image upload staging/finalization                                 | With temporary GUID folders |
| Dietary Tags (many-to-many)                                       | Via normalized `RecipeTags` join |
| Manual mapping system                                             | For total control and clarity |
| Full SCSS-based UI with theme switcher                            | **VoidGlass** custom components |


---

## 🚀 Tech Stack & Rationale

| Technology            | Reasoning                                                                 |
| ---------------------| -------------------------------------------------------------------------- |
| **Blazor Server**     | Seamless .NET full stack with real-time UI                                |
| **ASP.NET Core API**  | Clear separation, RESTful, Docker-ready                                   |
| **Entity Framework**  | Rapid dev with strong model-to-db mapping                                 |
| **Manual Mapping**    | Chosen over AutoMapper for transparency and intent                        |
| **VoidGlass Theme**   | Custom SCSS aesthetic to showcase visual polish                           |

---

## 🔍 Architecture Decisions

### 📁 Folder Structure

- `RecipeShare.Web`: Blazor frontend
- `RecipeShare.API`: ASP.NET Core API backend
- `RecipeShare.Models`: Shared DTOs and ViewModels

### 🔐 Identity Handling

- ASP.NET Core Identity with username-based login
- Demo user (`DemoUser` / `Password123!`) seeded on init

### 🧩 Data Modeling

- Tables: `Recipes`, `RecipeSteps`, `RecipeImages`, `RecipeFavourites`, `Tags`, `RecipeTags`
- Steps stored in both `StructuredSteps` and `Steps` string
- Images have captions, order, and cover flag
- Tags are normalized via join table for flexibility and metrics

---

## 📂 File Upload Strategy

### Problem:
Recipe ID is unknown at initial upload time.

### Solution:
- Uploads are staged in `/uploads/TempRecipeImages/{guid}/`
- On recipe creation, images are moved to:
  `/uploads/RecipeImages/{recipeId}/...`

### Notes:
- Uploads handled **in Blazor Web** (not API) to avoid CORS and simplify relative paths
- Paths stored in DB via `RecipeImages`

---

## 🎨 UI & UX Design

### ✨ VoidGlass Theme
- SCSS-based glass/neon UI
- Custom components: `vg-panel`, `vg-card`, `vg-input`, etc.
- Responsive layouts with tile/grid switching
- Dark/light toggle

### ❤️ Favourites
- Stored per user in `RecipeFavourites`
- Heart toggle + count updated live

### 📋 Markdown & PDF Export
- Markdown supports `[ ]` checklist rendering
- PDF generated via `html2pdf.js` from rendered recipe

### 🧱 Recipe Editor
- Dynamic form for steps, tags, and images
- Image preview + cover toggle
- Tag persistence on update
- Realtime validation via `EditForm` binding

---

## 🧑‍💻 Developer Notes

### Trade-offs & Constraints

| Area                    | Notes                                                                           |
| ----------------------- | ------------------------------------------------------------------------------- |
| AutoMapper              | Skipped in favor of `CustomMapper` for full control                             |
| Repository Layer        | Skipped — used `Service > DbContext` directly                                   |
| EF Lazy Loading         | Disabled — explicit `Include()` usage for clarity                               |
| Swagger in Production   | Left enabled intentionally for demo convenience                                 |
| Unit Testing            | Implemented basic xUnit test coverage for GET, POST, and validation flows       |
| Seed Dependency (Tests) | Tests rely on `DemoUser` and require seed data to be present prior to execution |

### Challenges

- Uploads before ID: handled via temp folder & GUID
- Syncing Steps string from `StructuredSteps`
- Non-destructive tag/image updates
- Ensuring safe rollback paths if submission canceled mid-flow

---

## 📈 Potential Enhancements (Post-Submission)

- [ ] Unit & integration tests
- [ ] Bulk insert on seed
- [ ] Ingredient-level filtering improvements (ALL vs ANY match)
- [ ] Image reordering via drag & drop
- [ ] Pagination & lazy loading

---

## 😎 Summary

**RecipeShare** is a fully-featured, visually polished recipe-sharing app demonstrating:

- Clean architecture and separation of concerns
- Fully implemented REST API with seed data and filtering
- Custom-designed Blazor UI with rich UX elements
- Manual mapping and data sync logic for clarity and control
- Real-world constraints tackled pragmatically (e.g., image handling, spec compliance)

> Built with performance, maintainability, and user experience at the forefront.

---

*Developed with ♥ by Donovan Minnie*
