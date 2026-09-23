## Öffentlicher Quellstand · 24.09.2026
 
 Der Quellcode dieses persönlichen Prototyps ist öffentlich einsehbar. Es wird keine neue MIT-/GPL- oder andere allgemeine Open-Source-Lizenz erteilt. Bestehende Rechte und Lizenzen an enthaltenen Drittanbieterkomponenten bleiben erhalten. Für weitergehende Nutzung oder Weitergabe bitte die jeweiligen Bedingungen beachten bzw. Jona kontaktieren.
 
 Die Releases sind experimentelle, vorhandene Buildstände. ZIP-Integrität und Prüfsummen sind geprüft; die Veröffentlichung ist keine neue Funktionsabnahme oder Zusicherung für produktive Arbeit. Private Profile, persönliche Daten und Zugangsdaten gehören nicht in dieses Repository.
 
 <!-- distribution-entry-20260924 -->
# FINORA

Lokale Stapelbearbeitung und Ausgabe von Renderings und Bildern.

[Website](https://tools.jano3dstudio.de/rendering-finish/) · [Repository](https://github.com/jano3dstudio/finora) · [Build und Download](DISTRIBUTION.md) · [Entwicklung](DEVELOPMENT.md)

Persoenliches Testprojekt / Prototyp von Jona Fynn Schlegelmilch. Idee, gestalterische Richtung und Optimierung von Jona; KI hat bei Umsetzung und Iterationen unterstuetzt. Kein zugesicherter produktiver Einsatz. Vor wichtigen Arbeiten eigene Sicherungen anlegen.

**Ablage:** Quellen und Anleitungen im Repository; ausfuehrbare Pakete als separate Release-Dateien. Repository ist öffentlich einsehbar. Oeffentliche Freigabe und Lizenzstatus: [PUBLICATION_REVIEW.md](PUBLICATION_REVIEW.md).
<!-- /distribution-entry-20260924 -->

# FINORA

Lokale Windows-App für das Finish von JPG-/PNG-Rendering-Serien. Beschreibender Arbeitstitel; öffentlich verfügbarer Pilot, keine Markenfreigabe behauptet.

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

Technisch geprüft mit synthetischen Testbildern. Jonas Praxistest mit echten Renderings sowie seine gestalterische Freigabe stehen aus. Landingpage, Windows-Download und Media Kit am 22.09.2026 veröffentlicht und per HTTPS geprüft.


## Website und Media Kit

[Produktseite](https://tools.jano3dstudio.de/rendering-finish/) · [Windows-Download](https://tools.jano3dstudio.de/rendering-finish/downloads/Rendering-Finish-0.1.0-Windows.zip) · [Media Kit](https://tools.jano3dstudio.de/rendering-finish/downloads/Rendering-Finish-0.1.0-Media-Kit.zip).

Webquellen, Media Kit und fertiges Website-Paket: `../../JS_Web/jano-tools-web/`. Integration: Tools-Startseite, jonaschlegelmilch.de Portfolio/Menü und /overview/. Öffentliches Windows-ZIP enthält nur EXE, Startanleitung und Lizenz; das lokale Quellpaket bleibt getrennt.

Produktname seit 23.09.2026: **FINORA**. Technische Ordner, Speicherkennungen und bestehende Startpfade bleiben kompatibel.


## GitHub-Ablage

FINORA – Lokale Stapelbearbeitung und Ausgabe von Renderings und Bildern.

Repository: `jano3dstudio/finora` (öffentlich einsehbar). Quellen, Build-Anleitung und Projektregeln werden versioniert. Persönliche Laufzeitdaten, Zugangsdaten und lokale Sicherungen gehören nicht in Git. Bestehende lokale Start- und Quellpfade bleiben erhalten. Der Upload ist eine Quellcodesicherung; technische Prüfstände und persönliche Freigabe stehen separat in der Projektdokumentation.
