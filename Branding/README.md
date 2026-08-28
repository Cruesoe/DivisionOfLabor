# Branding

Build assets for the Steam preview image. **Not shipped** — `deploy.ps1` ships an
allowlist (`1.6`, `About`), so this folder stays out of the Workshop download
automatically.

## Files

| File | Purpose |
|---|---|
| `preview-source.png` | Original 1280x698 artwork. The tick-box row is read out of this — treat it as the master art. |
| `compose-preview.ps1` | Regenerates `About/preview.png` from it. |

## Regenerating

    powershell -ExecutionPolicy Bypass -File .\Branding\compose-preview.ps1

With no arguments this reproduces the shipped image byte-for-byte.

## How it works

The tick boxes are lifted off `preview-source.png` as a transparent sprite —
luminance becomes the alpha channel, which separates the white artwork from the
black backdrop. That means they can be rescaled freely and are still the original
artwork, never redrawn. The backdrop and title are generated.

## Parameters

| Switch | Default | Notes |
|---|---|---|
| `-Cap` | `60` | Title cap height in px. The solver finds the font size that hits it. |
| `-BoxW` | `720` | Width of the tick-box row. Height follows at 1:8.8. |
| `-TitleY` | `278` | Y centre of the title caps. |
| `-BoxY` | `428` | Y centre of the box row. |
| `-Track` | `0.055` | Letter-spacing as a fraction of font size. |

## Decisions worth keeping

- **Arial Bold.** RimWorld renders its UI in Arial (Unity's default, baked into the
  game's assets — there are no loose font files to extract). Bold matches the weight
  of the tick-box strokes; the original's Light weight did not, which is why it read
  as spindly once enlarged.
- **Box-to-cap ratio 1.59.** At the original 2.04 the boxes visually dominated the title.
- **1280x720.** The original was 1280x698, which Steam letterboxes.
- **8-bit greyscale output with dither.** The art is greyscale, so 8-bit is visually
  identical and about a third the size — 288 KB against Steam's 1 MB preview cap
  (a 24-bit re-encode came out at 1.04 MB and would have been rejected). The +/-1
  dither stops the smooth backdrop gradient banding into rings when quantised.
- **Word spaces are set explicitly** at 0.34em. Drawing glyph-by-glyph for letter-spacing
  means `GenericTypographic` reports a space as ~0 wide, which collapses the words.
