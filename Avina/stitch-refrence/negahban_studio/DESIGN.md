# Design System Document: The Scholarly Vanguard

## 1. Overview & Creative North Star: "The Digital Manuscript"
This design system is built upon a Creative North Star we call **"The Digital Manuscript."** It is a synthesis of high-end, Notion-inspired minimalism—characterized by vast negative space and structured clarity—and the vibrant, kinetic energy of modern gamification. 

Unlike traditional educational platforms that feel "locked-in" to a grid, this system treats the screen as a living canvas. We break the "template" look by utilizing **intentional asymmetry** (heavy right-side weighting for RTL flow), **overlapping elements** (badges breaking container boundaries), and a **high-contrast typography scale** that makes even a simple mission list feel like a curated editorial piece. We are moving away from "software" and toward an "experience."

---

## 2. Colors: Tonal Depth over Structural Lines
Our palette uses a sophisticated Material 3 foundation, but our application is strictly editorial.

### The "No-Line" Rule
**Explicit Instruction:** Prohibit the use of 1px solid borders for sectioning or containment. Boundaries must be defined solely through background color shifts. Use `surface-container-low` (#eef1f3) sections sitting on a `surface` (#f5f7f9) background to define zones. 

### Surface Hierarchy & Nesting
Treat the UI as physical layers of fine paper and frosted glass.
- **Layer 0 (Base):** `surface` (#f5f7f9) — The foundation of the app.
- **Layer 1 (Sections):** `surface-container-low` (#eef1f3) — Large content areas.
- **Layer 2 (Interactive Cards):** `surface-container-lowest` (#ffffff) — Where the user takes action.
- **Layer 3 (Floating Overlays):** Use Glassmorphism (see below).

### The "Glass & Gradient" Rule
To elevate the "gamified" elements beyond flat vectors, use **Glassmorphism** for floating mission modals or inventory HUDs. Use `surface-container-lowest` at 70% opacity with a `24px` backdrop-blur. 
*   **Signature Textures:** For primary CTAs and progress bars, use a subtle linear gradient from `primary` (#4248d4) to `primary-container` (#9197ff) at a 135-degree angle. This adds "soul" and depth that prevents the UI from feeling sterile.

---

## 3. Typography: The RTL Authority
The typography is the backbone of the "Manuscript" feel. While the system uses **Plus Jakarta Sans** for Latin numerals and accents, the primary weight is carried by **Vazirmatn** (or IRANSans).

*   **Display (Lg/Md/Sm):** Used for level-up announcements and major module titles. These should be `ExtraBold` with a tight `-0.02em` letter spacing to feel impactful.
*   **Headline (Lg/Md/Sm):** Used for mission titles. High contrast between `headline-lg` and `body-md` is essential to create an editorial hierarchy.
*   **Body (Lg/Md/Sm):** Our "Notion" influence. Use `body-md` (#2c2f31) for lesson content with a generous `1.6` line-height to ensure readability for teenagers.
*   **Labels:** Always uppercase (for Latin) or `Medium` weight (for Persian) to distinguish them as metadata.

---

## 4. Elevation & Depth: Tonal Layering
We do not use shadows to create "pop"; we use light to create "presence."

*   **The Layering Principle:** Place a `surface-container-lowest` card (#ffffff) on a `surface-container-low` (#eef1f3) section. The delta in brightness creates a soft, natural lift.
*   **Ambient Shadows:** For floating elements (like a "Claim Reward" button), use an extra-diffused shadow: `0px 20px 40px rgba(44, 47, 49, 0.06)`. This mimics natural light rather than a harsh digital drop shadow.
*   **The "Ghost Border" Fallback:** If accessibility requires a stroke, use the `outline-variant` (#abadaf) token at **15% opacity**. Never use a 100% opaque border.

---

## 5. Components: Gamified Minimalism

### Cards & Mission Lists
*   **Rule:** Forbid divider lines. Separate mission items using 16px of vertical white space or a subtle hover shift to `surface-container-high` (#dfe3e6).
*   **Layout:** High-end asymmetry. The Mission Icon should slightly "overhang" the top-right corner of the card, breaking the bounding box.

### Buttons & Chips
*   **Primary Button:** Gradient-filled (`primary` to `primary-container`), `xl` roundedness (1.5rem).
*   **Secondary/Tertiary:** `surface-container-high` background with `on-surface` text. No border.
*   **Selection Chips:** Use `secondary-container` (#fcbcff) for active states to provide a "vibe" shift toward the gamified purple/indigo spectrum.

### Progress Trackers
*   **Visual Style:** Use a thick (12px+) track using `surface-container-highest` (#d9dde0). The fill should be the `primary` gradient. 
*   **Animation:** Progress bars must use a "Spring" easing function (e.g., `cubic-bezier(0.175, 0.885, 0.32, 1.275)`) to feel bouncy and rewarding.

### Gamification Accents (The "Jewels")
*   **Coins:** Use `tertiary` (#6c5a00) and `tertiary-container` (#ffd709) for a "burnished gold" look.
*   **Wisdom/XP:** Use `secondary` (#9720ab) to denote high-level achievement.
*   **Success States:** Use `on-tertiary-container` (#5b4b00) typography on `tertiary-container` backgrounds for high-legibility "Victory" banners.

---

## 6. Do's and Don'ts

### Do:
*   **Do** use RTL-specific spacing. The "gutter" should be larger on the right to anchor the Persian text.
*   **Do** use `xl` (1.5rem) corner radii for large containers to maintain a soft, friendly teenager-focused aesthetic.
*   **Do** embrace "White Space as a Feature." If a screen feels cluttered, increase the background-color-shift areas rather than adding lines.

### Don't:
*   **Don't** use pure black (#000000) for text. Always use `on-surface` (#2c2f31) to maintain the premium, soft-minimalist feel.
*   **Don't** use standard Material Design "Elevated Cards" with heavy shadows. Stick to tonal layering.
*   **Don't** center-align long-form Persian text. Always use right-alignment to maintain the "Digital Manuscript" editorial integrity.