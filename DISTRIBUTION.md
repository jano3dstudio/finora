# FINORA · Build und Lieferung

Stand: 24.09.2026. [Website](https://tools.jano3dstudio.de/rendering-finish/) · [GitHub-Releases](https://github.com/jano3dstudio/finora/releases).

## Vorbereiteter Build

`FINORA-Windows-20260924.zip` (326,710 Bytes), SHA-256 `5d1a8f69a22037a7d126d83fa0bc891d3e8c1b83dea78f02e88eea11e05c95fe`.

Quelle im lokalen App-Ordner: `FINORA.exe`.
Startweg des bestehenden Lieferstands: `FINORA.exe`.
Der Build ist ein vorhandener Lieferstand, kein frisch kompilierter oder erneut funktional abgenommener Build.
Paketintegritaet und Pruefsumme wurden geprueft. Die Release-Ablage wird separat bestaetigt; diese Datei behauptet keinen bereits erfolgten Upload.

## Selbst bauen

[Entwicklungsanleitung](DEVELOPMENT.md) · [Build-Einstieg](<_Projekt/build.ps1>).
Voraussetzungen, gepinnte SDKs und produktspezifische Tests stehen in der Entwicklungsanleitung.
Fuer gemeinsame Module benoetigt man gegebenenfalls das [MODULO-Repository](https://github.com/jano3dstudio/jano-app-kit); benachbarte Checkout-Ordner muessen den dort dokumentierten Namen behalten.
Der vorhandene lokale Build beweist keinen erfolgreichen Build aus einem frischen Checkout.

## Ordner und Daten

Quell-, Start- und Profilpfade bleiben stabil. Laufzeitdaten, Passwoerter, Testprofile und Sicherungen gehoeren nicht in Release-Pakete.
Alte Buildstaende bleiben lokal; fuer diese Lieferung wurde nur der oben genannte Kandidat ausgewaehlt.
EXE/ZIP-Dateien werden nicht in die Quellcode-Historie gezwungen. Getrennte Release-Assets enthalten SHA256SUMS.txt.

Vor oeffentlicher Weitergabe [PUBLICATION_REVIEW.md](PUBLICATION_REVIEW.md) beachten.

Startdatei im ZIP: `FINORA.exe`. ZIP zuerst vollständig entpacken.
