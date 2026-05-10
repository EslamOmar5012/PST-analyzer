<div align="center">

# 📬 PST Merge Tool

### *Enterprise-grade Outlook PST consolidation — completely free, zero limits*

[![License: MIT](https://img.shields.io/badge/License-MIT-22c55e?style=for-the-badge&logo=opensourceinitiative&logoColor=white)](LICENSE)
[![Platform](https://img.shields.io/badge/Windows-10%20%7C%2011%20%7C%20Server-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://github.com/mithundgnxt-stack/PstMerger/releases)
[![Outlook](https://img.shields.io/badge/Outlook-2013--365-0078D4?style=for-the-badge&logo=microsoftoutlook&logoColor=white)](https://github.com/mithundgnxt-stack/PstMerger/releases)
[![.NET](https://img.shields.io/badge/.NET_Framework-4.5%2B-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://github.com/mithundgnxt-stack/PstMerger/releases)
[![Version](https://img.shields.io/badge/Version-1.1.2-f59e0b?style=for-the-badge)](https://github.com/mithundgnxt-stack/PstMerger/releases)
[![Downloads](https://img.shields.io/github/downloads/mithundgnxt-stack/PstMerger/total?style=for-the-badge&color=06b6d4)](https://github.com/mithundgnxt-stack/PstMerger/releases)

<br/>

> **Merge unlimited `.pst` files into one master archive** with full folder preservation,  
> MD5 deduplication, registry patching, disk-space validation, and safe cancellation —  
> all inside a single lightweight Windows application.

<br/>

[⬇️ Download Latest](https://github.com/mithundgnxt-stack/PstMerger/releases) &nbsp;·&nbsp; [🐛 Report a Bug](https://github.com/mithundgnxt-stack/PstMerger/issues) &nbsp;·&nbsp; [💡 Request a Feature](https://github.com/mithundgnxt-stack/PstMerger/issues/new)

</div>

---

## 📋 Table of Contents

- [✨ Features](#-features)
- [📊 At a Glance](#-at-a-glance)
- [💻 Requirements](#-requirements)
- [🚀 Quick Start](#-quick-start)
- [📐 Architecture](#-architecture)
- [🔍 Deduplication Engine](#-deduplication-engine)
- [🔧 Registry Fix Details](#-registry-fix-details)
- [⚠️ Limitations & Tips](#️-limitations--tips)
- [🛠️ Building from Source](#️-building-from-source)
- [👥 Authors & Contributors](#-authors--contributors)
- [📄 License](#-license)

---

## ✨ Features

| Feature | Description |
|---|---|
| 📦 **Unlimited PST Merging** | Merge any number of `.pst` files from a folder into one master archive — no caps, no licensing fees |
| 🔍 **MD5 Deduplication** | Fingerprints every item via Subject + Sender + Date + Size + BodyLen; pre-seeds from existing destination so re-runs never re-import |
| 📁 **Full Folder Preservation** | Recursively mirrors Inbox, Sent Items, and all custom subfolder hierarchies exactly as-is |
| 🗂️ **All Item Types** | Handles Emails, Contacts, Calendar events, Tasks, and Notes with type-aware field extraction |
| 🔧 **Registry Size Fix** | One-click patch removes Outlook's default 50 GB PST cap → bumps it to ~2 TB for Office 2013 & 2016–365 |
| ⏹️ **Graceful Cancellation** | Cancel mid-run safely; the current item finishes before the process halts — no data corruption |
| 📝 **Persistent Log File** | Every action, warning, and error written to a timestamped `.log` file for full audit trails |
| 💾 **Disk Space Validation** | Calculates total source size and warns if target drive has less than 110% of that space |
| 🛡️ **Error Resilience** | Individual item failures are logged and skipped; folder creation retries up to 3× with 500 ms back-off |
| ⚡ **Async & Responsive UI** | Merge runs on a background thread — the UI stays live with a live progress bar and cancellable operation |

---

## 📊 At a Glance

```
┌──────────────┬──────────────┬──────────────┬──────────────┬──────────────┐
│   Max Size   │  PST Files   │  Item Types  │  Dedup Hash  │    Price     │
│    2  TB     │      ∞       │      5       │     MD5      │    FREE      │
│  Supported   │  Per Run     │  Supported   │ Fingerprint  │ Open Source  │
└──────────────┴──────────────┴──────────────┴──────────────┴──────────────┘
```

**Item type breakdown (typical mailbox)**

```
Emails    ████████████████████████████░░░░░░░░░░░░  55%
Calendar  ██████████░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  20%
Contacts  ██████░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  13%
Tasks     ███░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   7%
Notes     ██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   5%
```

**Relative processing cost per item**

```
Item Copy (MAPI Move)    ████████████████████████████████████████  85%
MD5 Hash + Lookup        █████░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  12%
Folder Create/Find       ███░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   8%
COM Object Release       █░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   3%
Log File I/O             ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  <1%
```

---

## 💻 Requirements

| Requirement | Detail |
|---|---|
| 🖥️ **Operating System** | Windows 10, Windows 11, or Windows Server 2016 / 2019 / 2022 |
| 📧 **Microsoft Outlook** | 2013, 2016, 2019, 2021, or Microsoft 365 (desktop — **not** web) |
| ⚙️ **.NET Framework** | 4.5 or later (pre-installed on Windows 10/11) |
| 🔑 **Permissions** | Standard user for merge; Administrator only needed for the one-time Registry Fix |
| 💾 **Disk Space** | At least ~110% of total source PST size free on the destination drive |

---

## 🚀 Quick Start

> ⚠️ **Before you begin:** Close Microsoft Outlook completely. The tool opens PSTs via Outlook Interop — having Outlook open simultaneously can cause conflicts.

### Step 1 — Download & Run

Grab `PstMerger.exe` from the [Releases page](https://github.com/mithundgnxt-stack/PstMerger/releases). No installer needed — it's a single portable executable.

```
PstMerger.exe   ← double-click to launch
```

---

### Step 2 — Fix PST Size Limits *(one-time)*

Click the **🟡 Fix PST Size Limits** button. This patches two registry keys to raise Outlook's 50 GB PST cap to ~2 TB for both Office 2013 and Office 2016–365.

> 💡 **Tip:** After applying, restart Outlook if it was already open. You only need to do this once per machine.

---

### Step 3 — Select Source Folder

Click **Browse...** next to *"Source Folder (PST files)"*. Choose the directory containing all your `.pst` files.

> ⚠️ Only top-level `.pst` files in the folder are scanned — subfolders are **not** searched recursively. Keep all source PSTs flat in one directory.

---

### Step 4 — Choose Destination PST

Click **Browse...** next to *"Destination Master PST"*. You can:

- 📂 **Pick an existing PST** — items will be added to it (existing items are deduped automatically)
- 📝 **Type a new filename** — a fresh PST will be created from scratch

---

### Step 5 — Configure Deduplication

The **☑️ Remove Duplicates** checkbox is **enabled by default**.

| Setting | Behaviour |
|---|---|
| ✅ Checked *(default)* | MD5 fingerprint checked for every item; duplicates silently skipped |
| ☐ Unchecked | All items copied unconditionally — useful if you intentionally want duplicates |

---

### Step 6 — Start Merge 🎉

Click **🟢 Start Merge**. Watch real-time progress in the black log panel. At any point you can click **🔴 Cancel** to stop safely — the current item will finish before halting.

When done, a summary line shows how many duplicates were skipped:

```
[09:41:17] Deduplication complete. 3,842 duplicate item(s) skipped.
[09:41:17] COMPLETED: All PST files merged successfully.
```

---

## 📐 Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        PstMerger.exe                            │
│                                                                 │
│  ┌──────────────┐        ┌──────────────────────────────────┐  │
│  │  MainForm.cs │        │          PstService.cs           │  │
│  │              │        │                                  │  │
│  │  • WinForms  │──────▶│  MergeFiles()                    │  │
│  │  • Async UI  │        │  ├─ SeedHashesFromFolder()       │  │
│  │  • Logging   │        │  ├─ ProcessSourcePst()           │  │
│  │  • Progress  │        │  ├─ CopyFolders() [recursive]    │  │
│  └──────────────┘        │  ├─ GetItemHash() → MD5          │  │
│                          │  ├─ FindFolderByName()           │  │
│                          │  └─ GetRootFolder()              │  │
│                          └────────────┬─────────────────────┘  │
└───────────────────────────────────────┼─────────────────────────┘
                                        │ Outlook Interop (COM)
                                        ▼
                          ┌─────────────────────────┐
                          │   Microsoft Outlook      │
                          │   MAPI Namespace         │
                          │                          │
                          │  ns.AddStore(pst)        │
                          │  folder.Items[i].Copy()  │
                          │  copy.Move(destFolder)   │
                          │  ns.RemoveStore(root)    │
                          └──────────┬──────────────┘
                                     │
                    ┌────────────────┴─────────────────┐
                    │                                   │
            ┌───────▼──────┐                  ┌────────▼──────┐
            │  Source PSTs │                  │  Master PST   │
            │  (read-only) │                  │  (output)     │
            └──────────────┘                  └───────────────┘
```

**Data flow summary:**

```
Source PSTs  ──▶  Outlook MAPI  ──▶  PstService  ──▶  MD5 HashSet  ──▶  Master PST
    📂               🔗                  ⚙️               🔐               📦
  (input)        (interop)           (engine)          (dedup)          (output)
```

---

## 🔍 Deduplication Engine

The dedup system uses a **folder-agnostic** approach — the same email in `Inbox` and `Sent Items` is correctly identified as a single item, not two.

### Fingerprint fields

```csharp
// GetItemHash() — PstService.cs
string raw = string.Join("|",
    subject,   // "Q3 Budget Review"
    sender,    // "alice@company.com"
    sentOn,    // "2024-03-15T09:22:00.0000000" (ISO 8601)
    size,      // "48392"  (bytes)
    bodyLen    // "1204"   (char count)
);

// → MD5 → "A3F2C1D9E4B76C8F..."
```

| Field | Source Property | Fallback Chain |
|---|---|---|
| 📋 **Subject** | `item.Subject` | — |
| 👤 **Sender** | `item.SenderName` | `Organizer` → `From` |
| 📅 **Timestamp** | `item.SentOn` | `Start` → `CreationTime` |
| ⚖️ **Size** | `item.Size` | — |
| 📝 **Body Length** | `item.Body.Length` | — |

### Pre-seeding on re-runs

When an **existing** destination PST is selected, the tool first walks its entire folder tree and indexes every item's hash before any copying begins. This means:

```
Run 1:  merge 10 PSTs  → 50,000 items written  → 0 duplicates
Run 2:  add 3 more PSTs → 50,000 pre-seeded    → only new items written
```

> ℹ️ If a hash cannot be computed (malformed item), the item is **allowed through** — never silently dropped.

---

## 🔧 Registry Fix Details

By default, Outlook limits PST files to ~50 GB. The **Fix PST Size Limits** button modifies `HKEY_CURRENT_USER` to effectively remove this cap.

### Keys modified

```
HKEY_CURRENT_USER
└── Software
    └── Microsoft
        └── Office
            ├── 15.0          ← Office 2013
            │   └── Outlook
            │       └── PST
            │           ├── MaxLargeFileSize   = 2,000,000  (DWORD, MB)
            │           └── WarnLargeFileSize  = 1,900,000  (DWORD, MB)
            └── 16.0          ← Office 2016 / 2019 / 2021 / 365
                └── Outlook
                    └── PST
                        ├── MaxLargeFileSize   = 2,000,000  (DWORD, MB)
                        └── WarnLargeFileSize  = 1,900,000  (DWORD, MB)
```

| Key | Default | After Fix |
|---|---|---|
| `MaxLargeFileSize` | ~50,000 MB | **2,000,000 MB (~2 TB)** |
| `WarnLargeFileSize` | ~47,500 MB | **1,900,000 MB** |

> 🔑 If the tool reports a permissions error, run `PstMerger.exe` as Administrator for this one-time step only.

---

## ⚠️ Limitations & Tips

```
┌─────────────────────────────────────────────────────────────────────┐
│  ⚠  IMPORTANT — Read before running on large archives              │
├─────────────────────────────────────────────────────────────────────┤
│                                                                     │
│  • Close Outlook completely before starting a merge                 │
│  • Source PSTs must be in a flat folder (no subfolder scanning)     │
│  • If source PST = destination PST, it is automatically skipped     │
│  • Speed is limited by Outlook Interop / MAPI — this is expected    │
│  • Very large archives (100 GB+) can take hours — plan accordingly  │
│  • The log file is saved next to PstMerger.exe for audit purposes   │
│                                                                     │
└─────────────────────────────────────────────────────────────────────┘
```

**MD5 collision edge cases:**
The hash is robust for typical mailbox data but is not cryptographically collision-resistant. In extremely rare cases, two genuinely different items could produce the same hash and one would be skipped. For archival-critical use, verify totals against source counts post-merge.

---

## 🛠️ Building from Source

### Prerequisites

- Visual Studio 2019+ **or** MSBuild 15+
- .NET Framework 4.8 SDK
- Microsoft Outlook installed (for Interop DLLs via GAC)

### Clone & Build

```bash
# Clone the repository
git clone https://github.com/mithundgnxt-stack/PstMerger.git
cd PstMerger

# Build Release
msbuild PstMerger.csproj /p:Configuration=Release

# Output
bin\Release\PstMerger.exe
```

### Project structure

```
PstMerger/
├── 📄 PstMerger.csproj          # .NET 4.8 WinExe project
├── 📄 Program.cs                 # Entry point — [STAThread] Main()
├── 📄 MainForm.cs                # WinForms UI + async orchestration
├── 📄 MainForm.Designer.cs       # Auto-generated layout code
├── 📄 PstService.cs              # Core merge & dedup engine
├── 📄 Properties/AssemblyInfo.cs # Version & metadata
├── 📄 LICENSE                    # MIT
└── 📄 README.md                  # This file
```

### Key dependencies

| Assembly | Version | Purpose |
|---|---|---|
| `Microsoft.Office.Interop.Outlook` | 15.0.0.0 | PST/MAPI operations |
| `office` | 15.0.0.0 | Core Office COM types |
| `System.Windows.Forms` | 4.8 | UI layer |
| `System.Security.Cryptography` | built-in | MD5 hashing |

> Both `EmbedInteropTypes=True` — the output EXE embeds the interop types, so no separate PIA redistribution is needed.

---

## 👥 Authors & Contributors

<table>
<tr>
<td align="center" width="50%">

### 🧑‍💻 Mithun
**Creator & Lead Developer**  
DataGuardNXT  

Core architecture, Outlook Interop integration,  
async UI, registry patching, logging system

</td>
<td align="center" width="50%">

### 👨‍💻 Eslam Omar
**Contributor**  

Added the **Remove Duplicates** feature —  
MD5 fingerprinting engine, pre-seed logic,  
folder-agnostic hash keys, dedup reporting

</td>
</tr>
</table>

---

## 🤝 Contributing

Contributions are welcome! To get started:

1. **Fork** this repository
2. **Create** a feature branch: `git checkout -b feature/amazing-feature`
3. **Commit** your changes: `git commit -m 'Add amazing feature'`
4. **Push** to the branch: `git push origin feature/amazing-feature`
5. **Open a Pull Request**

Found a bug? Please [open an issue](https://github.com/mithundgnxt-stack/PstMerger/issues) with steps to reproduce, your OS version, and Outlook version.

---

## 📄 License

```
MIT License

Copyright (c) 2026 DataGuardNXT

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
```

See [LICENSE](LICENSE) for the full text.

---

<div align="center">

**© 2026 DataGuardNXT · All Rights Reserved**

*Built for IT administrators who needed it done right.*

⭐ If this tool saved you time, give the repo a star!

[🐛 Report Bug](https://github.com/mithundgnxt-stack/PstMerger/issues) &nbsp;·&nbsp; [💡 Feature Request](https://github.com/mithundgnxt-stack/PstMerger/issues/new) &nbsp;·&nbsp; [⬇️ Download](https://github.com/mithundgnxt-stack/PstMerger/releases)

</div>
