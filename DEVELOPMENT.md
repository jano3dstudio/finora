# Entwicklung

## Einstieg

README.md, PROJECT_MAP.json, AGENTS.md und _Projekt/docs/AUFTRAG.md. Fachlicher Umfang: lokales Serienfinish; keine generative Bildbearbeitung.

## Voraussetzungen

Windows x64, .NET Framework 4.x Compiler (`Framework64/v4.0.30319/csc.exe`), WebView2 Runtime. Mitgelieferte WebView2-SDK-DLLs unter `_Projekt/deps` samt Lizenz und Herkunft. Node für reine Logiktests; Python/Pillow optional für unabhängige Dateiprüfung und Icon-Neubau.

## Build

```powershell
./_Projekt/build.ps1
```

Benötigt `jano-app-kit` als Nachbarordner. Alternativ `-KitRoot <Pfad>` und `-OutputPath <neue-EXE>`. Keine Kompilierung über laufende App. Gemeinsamer Build erstellt eine neue temporäre EXE, prüft unveränderte Kit-Eingaben und liefert EXE plus SHA256-Lock. Bevorzugt in frischem Review-Ordner bauen und erst nach Prüfung liefern.

## Aufbau

- `ui/engine.js`: deterministische Pixelkorrekturen, LUT-Parser/trilineare Interpolation, Namensbildung; browserunabhängig testbar.
- `ui/app.js`: Import, Vorschauliste, Einzel-/Serienzustand, Rendering, Branding, IndexedDB-Vorlagen, Exportfortschritt und Abbrechen.
- `ui/locales.js`: semantische DE/EN-Kataloge. `ui/app.css`: nur Fachlayout; Kit-Tokens/Komponenten bleiben gemeinsam.
- `native/Desktop.cs`: Ressourcenstart, gesperrter lokaler WebView2-Ursprung, begrenzte Dateibrücke, native QA. Kit stellt Fensterrahmen und gemeinsamen Look-Speicher.

## Tests

```powershell
node ./_Projekt/tests/engine.test.cjs
./_Projekt/tests/start-qa.ps1 -OutputPath <neuer-testordner>
./_Projekt/tests/start-qa.ps1 -OutputPath <neuer-maustestordner> -Mouse
python ./_Projekt/tests/verify-exports.py <testordner>
```

GUI-Tests benötigen einen erreichbaren Windows-Desktop. Mausprüfung nicht parallel zu anderer Computerbedienung ausführen. Normale Nutzerprofile werden nicht gelesen oder verändert. Ergebnisse siehe VERIFICATION.md. 150%-DPI ist nicht durch Browser-Skalierung bewiesen.

## Daten

Produktprofil: `%LOCALAPPDATA%/JANO/RenderingFinish/profile`; Kit-Looks separat `%LOCALAPPDATA%/JANO/AppKit/looks.json`. Rendering-Originale nur lesen. Keine Cloud, kein Server-Port, keine Telemetrie. Exportdateien atomar via Tempdatei im gewählten Zielordner; vorhandene Dateien niemals ersetzen. Persönliche Daten und generierte Pakete nicht committen.

## Zusammenarbeit / offene Punkte

Jonas Freigabe für Bedienung und Bildlook, echter Kundenbildsatz, weitere Windows-/DPI-Umgebungen offen. Kein öffentlicher Produktname festgelegt. Neue Hersteller-/SDK-Versionen erst mit eigener Prüfung übernehmen. Arbeitskopie in JS_GPT_Projects ist Vorbereitung, nicht kanonische Weiterentwicklungsquelle nach Auslieferung.
