# Rendering Finish · 0.1.0

Lokale Windows-App für das Finish von JPG-/PNG-Rendering-Serien. Arbeitsname, kein öffentlich eingeführter Produktname.

**Start:** `Rendering Finish.exe` in diesem Ordner. Benötigt Windows x64 mit Microsoft Edge WebView2 Runtime. Keine Anmeldung, kein Abo und kein Bild-Upload.

1. Bilder über **Bilder hinzufügen** oder Drag-and-drop laden.
2. Bildlook wählen und Regler anpassen. **Ganze Serie** ändert den jeweiligen Regler bei allen Bildern; **Nur dieses Bild** erstellt eine individuelle Korrektur.
3. Unter **Branding** ein PNG-Logo, Text und optionale Gestaltung hinzufügen. Position über neun Anker, X/Y-Regler oder direktes Ziehen im Bild einstellen.
4. Bei Bedarf unter **Kundenvorlagen** den aktuellen Bildlook, Logo/LUT-Kopien, Gestaltung und Exportoptionen speichern.
5. Bilder links für den Export auswählen, **Serie exportieren**, Format/Größe wählen und Ausgabeordner bestätigen.

Originale werden nicht überschrieben. Exporte erhalten `_finish`; bei Namenskonflikten folgen `_2`, `_3` usw. Abbrechen erhält bereits abgeschlossene Dateien. Die Bildserie ist sitzungsbezogen; gespeicherte Kundenvorlagen bleiben nach Neustart erhalten.

## Bildverarbeitung

- 8-Bit-JPG und PNG, bis 80 Megapixel pro Bild, maximal 100 Bilder pro Serie; sequenzielle Verarbeitung.
- Automatik gleicht mittlere Helligkeit zurückhaltend aus (maximal ±1 EV vor Stärkeregler). Sie beurteilt keine Bildgestaltung und ersetzt nicht Jonas Sichtprüfung.
- Belichtung, Wärme/Tönung, Kontrast, Lichter/Schatten, Sättigung; sechs mitgelieferte Looks und reine 3D-`.cube`-LUTs mit 2–65 Stützstellen und optionalem DOMAIN_MIN/MAX. Keine 1D-Shaper, Log-/HDR-Konvertierung oder Adobe-Engine.
- Verarbeitung auf einer sRGB-Canvas. Farbprofile werden beim Dekodieren vom WebView2-Browser behandelt; Ausgabe ist für sRGB/Screen bestimmt. Kein ICC-Proofing und keine Zusicherung pixelidentischer Metadatenübernahme. EXIF/IPTC werden nicht übernommen; orientierte JPGs werden vom Browser dekodiert.
- PNG-Transparenz bleibt erhalten. JPG erhält die gewählte Hintergrundfarbe. Maximalgröße betrifft die lange Kante, ohne Hochskalieren oder Beschnitt.
- Logo, Text und Gestaltung werden nach der Farbkorrektur und Skalierung eingesetzt. PNG-Logo und Text bleiben von der Bild-LUT unbeeinflusst.
- Vorschau maximal 1400 px; Export verwendet die gewählte Original-/Zielauflösung. LUT-Vorschaukacheln zeigen den Look bei voller Stärke auf dem Originalbild.

## Daten und Gestaltung

Kundenvorlagen liegen lokal in IndexedDB im WebView2-Profil unter `%LOCALAPPDATA%/JANO/RenderingFinish/profile`. Sie enthalten eingebettete Logo-/LUT-Kopien, Texte und Parameter, keine Rendering-Serie. Für eine Sicherung die App schließen und den Profilordner sichern. Private Profile gehören nicht ins Quellpaket.

**Oberfläche** verwendet den zentralen JANO-Look-Editor. Gemeinsame Oberflächen-Looks liegen in `%LOCALAPPDATA%/JANO/AppKit/looks.json`; **Eigener Look für diese App** ist verfügbar. Oberflächen-Looks verändern niemals Bilder oder Kundenvorlagen.

App-Kit 0.2.0, Module `window`, `web`, `looks`, `mouse-qa`; konkrete Quellrevision und Hashes stehen neben der EXE in `Rendering Finish.exe.kit-lock.json`. Die EXE ist unabhängig vom Quell-Kit lauffähig.

Quellen: [_Projekt](./_Projekt/) · [Entwicklung](./DEVELOPMENT.md) · [Prüfbericht](_Projekt/VERIFICATION.md) · [Projektkarte](./PROJECT_MAP.json).

Technisch geprüft mit synthetischen Testbildern. Jonas Praxistest mit echten Renderings sowie seine gestalterische Freigabe stehen aus. Kein Remote-Upload, keine Veröffentlichung.
