# Division of Labor

RimWorld 1.6 mod.

Splits RimWorld's broadest work types into focused jobs, so you can assign specialists instead of generalists. XML only — no assembly, no Harmony.

## New work types

| Work type | Priority | Split out of | Covers |
|---|---|---|---|
| **Nurse** | 1250 | Doctor, Warden | Rescuing, feeding colonists / prisoners / animals, hemogen, visiting the sick |
| **Butcher** | 975 | Cooking | Butcher table bills — meat and kibble |
| **Deconstruct** | 925 | Construction | Deconstruct, uninstall, remove floors, roofs and foundations |
| **Repair** | 875 | Construction | Repair, fix broken-down buildings |

Doctors keep all tending and surgery. Construction keeps building.

## Reordering

Related jobs are moved to sit together in the Work tab:

- **Fishing** 350 → 930, next to Hunting
- **Plant cutting** 500 → 650, next to Growing
- **Dark study** folded into Research as its first job
- **Smoothing** demoted within Construction, walls (20) before floors (10)

## Supported mods

Folded in rather than left as separate columns, and only when the mod is present:

| Mod | Change |
|---|---|
| Hospitality | Recruiter and Diplomat join Warden |
| Therapy | Therapy joins Warden |
| Simple Improve | Improving joins Construction as its lowest job |
| Recycle This | Recycle and destroy join Crafting |
| Vanilla Books Expanded | Writing moved to 420 |
| Dubs Bad Hygiene | Patient-care jobs join Nurse |
| Vehicle Framework | Repair / disassemble vehicle join Repair / Deconstruct |
| VE Gravship | Maintain gravship joins Repair |
| Guarding Pawns + Keyz Allow Utilities | Finish off joins guard duty |

Detection is by `PatchOperationConditional` on the def being patched, not by mod name or packageId, so forks and renames keep working.

## Layout

```
1.6/Defs/WorkTypes.xml     the four work types
1.6/Patches/               one file per work type, named <naturalPriority>_<source>_<name>
Branding/                  preview image source + generator (not shipped)
deploy.ps1                 copies 1.6/ and About/ to the RimWorld Mods folder
```

Patch filenames are prefixed with the work type's `naturalPriority`, so a directory listing reads as a map of the Work tab. Empty files are placeholders marking a work type this mod does not currently touch.

## Building

This repo is the master. To deploy into RimWorld:

```powershell
powershell -ExecutionPolicy Bypass -File .\deploy.ps1        # -WhatIf for a dry run
```

It ships an allowlist (`1.6`, `About`) and hash-verifies every file, so build assets never reach the Workshop.

## Compatibility

Safe to add to an existing save. Removing it mid-save resets the affected work priorities.

Incompatible with Complex Jobs, Nurse Job, and Smooth Walls First — they change the same work types.

## Licence

MIT
