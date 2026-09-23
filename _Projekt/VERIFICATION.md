# Prüfstand · 22.09.2026 · Version 0.1.0

## Technisch nachgewiesen

- Native Windows-EXE mit identischer eingebetteter Oberfläche und JANO App-Kit 0.2.0, Revision `shared-looks.1-chrome.2-controls.1`. Konkrete Eingabe-/EXE-Hashes im Liefer-Lock.
- Echter WebView2-Lauf (153.0.4234.48), isolierte Testprofile: zehn PNGs importiert, JPG zusätzlich, Einzelkorrektur und Serienwerte, Logo/Text, PNG-Serienexport.
- Kundenvorlagen mit eingebettetem PNG-Logo und 3D-LUT in IndexedDB gespeichert; nach echtem Seiten-Neuladen vorhanden. Sprachwechsel erhält Daten. Oberfläche und Bildlooks bleiben getrennt.
- Unabhängige Pillow-Prüfung von 13 Ausgabedateien: gültige PNG/JPG-Signaturen und Dekodierung, erwartete Bildmaße, PNG-Alpha, weißer JPG-Hintergrund; großes JPG von 2600 × 1500 nach 1280 × 738 verkleinert.
- Wiederholter Export erzeugt zusätzlichen Namen; erstes und zweites PNG bytegleich, keine Überschreibung. Abbrechen hinterlässt keine halbfertigen Ausgabedateien.
- Logo bleibt von der Farbkorrektur ungetönt; Originalansicht blendet Branding aus. LUT-Interpolation/Rot-Grün-Blau-Reihenfolge, neutrale Pixelidentität, Belichtung, Alphakanal und ungültige LUTs separat getestet.
- DE/EN-Schlüsselgleichheit geprüft. Hauptansicht, leerer Zustand, Branding, Kit-Lookdialog und kompakte Ansicht angesehen. Überlauf der schmalen Bildliste korrigiert.
- Native Fensterprüfung mit echten SendInput-Mausklicks bei 96 DPI/100 %: erster Maximieren-Klick, dreimal Maximieren/Wiederherstellen, Minimieren aus beiden Zuständen, unveränderte Normalgröße. Autorenzeile unten links im nativen Fenster sichtbar.

## Grenzen und persönliches Review

Synthetische Bilder und Testlogo; kein echter Kundenbildsatz wurde als Freigabe verwendet. Jonas Bildlook-, Bedienungs- und Designabnahme steht aus. Weitere Windows-Rechner und 150%-DPI sind nicht geprüft. Die automatische Korrektur gleicht Helligkeit aus, bewertet keine gestalterische Qualität. Kein Adobe Camera Raw und kein professionelles ICC-Proofing.

Die native Mausprüfung enthält einen Desktop-Screenshot mit fremden Overlays; dieser Screenshot wird nicht ins Produktpaket übernommen. Saubere WebView-Aufnahmen liegen separat im Prüfverzeichnis.

Die Erstlieferung war lokal. Aufgrund des Folgeauftrags wurden Produktseite, Downloads und Website-Verknüpfungen am 22.09.2026 veröffentlicht. Kein Remote-Git-Repository erstellt.

## Belege

`review/delivered/`: End-to-end-Bericht, JSON-Ergebnisse, unabhängige Exportprüfung und WebView-Aufnahmen der tatsächlichen Liefer-EXE.
`review/mouse/`: Maus-Prüfbericht der tatsächlichen Liefer-EXE (keine fremden Desktop-Overlays als Produktmedien).
`Rendering Finish.exe.kit-lock.json` im App-Einstieg: gehashter Build- und Kit-Stand.

Reproduzieren: DEVELOPMENT.md. Vor jeder neuen Testserie frischen Testordner verwenden.

Quellpaket separat im Arbeitsbereich entpackt und ohne Zugriff auf das produktive Kit erfolgreich neu gebaut (53 Dateien mit SHA256-Manifest). Kanonische Projektkarte, Toolbox-Katalog und Kit-Anbindung geprüft.

## Web-/Medienlieferung · 22.09.2026

QA-Endstatus vor Screenshot auf neutralen Bereit-Text gesetzt; keine Produktfunktion geändert. Neue Liefer-EXE vollständig im tatsächlichen WebView2-End-to-end-Test geprüft (`review/public-media/PASS.txt`). Öffentliche Screenshots enthalten keine lokalen Exportpfade. Frühere Mausprüfung bleibt auf ihrem dokumentierten Build; keine neue DPI-Freigabe behauptet.

Landingpage DE/EN, 320/390/768/1440 px im Edge-Browser, Bewegungspause, Reduced Motion, statische Inhalte ohne JavaScript, MP4-Wiedergabe, ZIP-Downloads und drei Website-Verknüpfungen geprüft. Kein realer iPhone/Safari-Test. 27 hochgeladene Dateien per HTTPS bytegenau mit den freigegebenen Uploadpaketen verglichen. Belege: `../../../JS_Web/jano-tools-web/review/rendering-finish/`.

Media Kit: DE/EN-Texte, Icons, drei echte App-Aufnahmen, Social-Motive 16:9/1:1/9:16, stumme 16-Sekunden-Screenshot-Trailer 16:9/9:16, Untertitel, Herkunfts-/Nutzungshinweise und SHA256. Synthetische Testbilder, keine Kundenreferenzen; keine generativen Fotos, Stimmen oder Musik.
