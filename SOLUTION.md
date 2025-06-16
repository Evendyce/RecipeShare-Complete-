# 🔧 RecipeShare — SOLUTION.md

This document outlines the architectural decisions, implementation rationale, trade-offs, and personal reflections behind the development of **RecipeShare**, a take-home assessment project.

---

## 🌍 Project Goals

* Build a **recipe-sharing platform** with CRUD support.
* Enable user interaction through favourites, filtering, and step tracking.
* Apply clean design and **custom UI theme** (VoidGlass).

---

## ✅ Requirements from Spec (Implemented)

* [x] Register/Login with user system
* [x] Create, Read, Update, Delete Recipes
* [x] Support multiple images per recipe
* [x] Mark one image as a cover
* [x] Filter recipes by tags
* [x] Export to PDF or Markdown
* [x] Use .NET, Entity Framework, and a frontend framework (Blazor)
* [x] Include default seed data

---

## ✨ Additional Features (Beyond Spec)

* [x] Structured step editing (with automatic sync to flat `Steps` string)
* [x] Per-user Favourite toggles with counters
* [x] Interactive checklist view for recipe steps
* [x] Responsive tile grid with filters and tag badges
* [x] Full UI/UX built with custom **VoidGlass** theme (SCSS-based)
* [x] Toggle cover image from UI
* [x] Image staging/finalization logic with temporary folders
* [x] Dietary Tags as a many-to-many normalized entity (`RecipeTags`)
* [x] Smart tag persistence on update (non-destructive sync)

---

## 🚀 Technology Stack & Rationale

| Technology           | Reasoning                                                                 |
| -------------------- | ------------------------------------------------------------------------- |
| **Blazor Server**    | Clean full-stack .NET with real-time interaction, no JS bundling needed   |
| **ASP.NET Core API** | Clean API separation, Docker-ready, testable                              |
| **Entity Framework** | Rapid development, clean migrations, flexible mapping control             |
| **Manual Mapping**   | Chosen over AutoMapper for clarity and full control during DTO transforms |
| **VoidGlass Theme**  | Custom SCSS-based aesthetic to showcase design capability                 |

---

## 🔍 Architecture Decisions

### 📑 Folder Structure

* Clear separation of concerns:

  * `RecipeShare.Web`: Blazor frontend
  * `RecipeShare.API`: Web API backend
  * `RecipeShare.Models`: Shared DTOs

### 🔗 Identity Handling

* Used `Username`-based login for simplicity
* ASP.NET Core Identity with seeded `DemoUser`

### 📊 Data Modeling

* `Recipes`, `RecipeSteps`, `RecipeImages`, `RecipeFavourites`, `Tags`
* Dietary tags use a **normalized many-to-many** relationship (`RecipeTags`)
* Steps and Images support **ordered sequences**
* Steps are dual-stored: both `StructuredSteps` and joined `string Steps` field for spec compliance

---

## 📂 File Upload Strategy

### 🔍 Challenge:

Recipe IDs aren’t available until DB creation.

### ✅ Solution:

* Uploads use **temp folder** `/uploads/TempRecipeImages/{guid}` before creation
* After `POST`, image paths are finalized and moved to:
  `/uploads/RecipeImages/{recipeId}/...`
* Ensures paths are correct and avoids orphaned files

### 🔄 API vs Web Upload:

* Image uploads are handled by the Web frontend project, not via API
* Rationale: avoids CORS complexity, simplifies relative URL references, and aligns with hosting expectations

---

## ✨ UI & UX Design

### 🚀 VoidGlass Theme

* Fully custom SCSS layout
* Blur + neon accent design
* Includes:

  * `vg-button`, `vg-card`, `vg-panel`, `vg-input`, `vg-heading`, etc.
  * Responsive layout grid for Recipe Tiles

### 💍 Favourite Toggle

* Per-user tracking using `RecipeFavourites`
* Heart toggle icon updates count + style

### ✏️ Markdown + PDF Export

* Client-side Markdown export with `[ ]` checkbox support
* PDF export via `html2pdf.js` from rendered content

### 🛠️ Recipe Editor

* Structured Step builder
* Cover + optional image uploader
* Dynamic Tag selector
* Validation integrated with EditForm

---

## 🧑‍💻 Developer Notes

### Trade-offs & Constraints

| Area                      | Notes                                                                |
| ------------------------- | -------------------------------------------------------------------- |
| AutoMapper                | Avoided in favor of `CustomMapper` for transparency                  |
| Full Repository Layer     | Skipped, used `Service > DbContext` directly per personal preference |
| EF Lazy Loading           | Avoided for clarity, used explicit `Include()`s                      |
| API Swagger in Production | Enabled for demo purposes (comment for prod)                         |
| Full Test Suite           | Not implemented, focus was core features                             |

### Challenges

* Managing file uploads without a pre-existing Recipe ID
* Ensuring image paths persist cleanly through create/update flows
* Avoiding duplication during update of steps/images/tags

---

## 📈 Possible Enhancements (Post-Submission)

* [ ] Add `Unit Tests` for API and Services
* [ ] Add `Bulk Insert` logic during seeding
* [ ] Expand filtering (e.g., ingredients contains ALL vs ANY)
* [ ] Support drag & drop image reordering
* [ ] Add paging to recipe tiles

---

## 😎 Summary

RecipeShare demonstrates:

* Clean architectural foundations
* End-to-end feature implementation
* Custom styling and UX polish
* Practical problem solving under realistic constraints

> The app was built with performance, usability, and maintainability in mind.

Thank you for reviewing this project! 🌟

---

*Developed by Donovan Minnie*
