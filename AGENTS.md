# Rendering Finish

Zuerst PROJECT_MAP.json, DEVELOPMENT.md, _Projekt/VERIFICATION.md und _Projekt/docs/AUFTRAG.md lesen. Kanonisch: JS_GitHub/rendering-finish. Keine Rückübernahme alter Staging-Kopien ohne Vergleich.

App-Kit-Quelle ../jano-app-kit: DESIGN_SYSTEM.md, SHARED_DEVELOPMENT.md, SHARED_LOOKS.md. Module werden über _Projekt/kit.ref.json direkt beim Build eingebunden. Keine eigenen Kopien gemeinsamer Fenster-/Look-Komponenten bearbeiten. Gemeinsame Änderungen bewusst prüfen und Builds getrennt liefern.

Fachlogik in engine.js; DOM, Bilddecodierung, Branding und Vorlagen in app.js; DE/EN-Texte in locales.js. Native Bridge darf nur vom festen lokalen Ursprung angesprochen werden. Export ausschließlich in vom Nutzer gewähltem Ordner; niemals Originale überschreiben. Dateinamenbegrenzung, atomare Dateiablage und Kollisionsschutz erhalten.

Keine echten Kundendaten, Bilder oder privaten Profile in Tests, Git oder Paketen. Tests mit --self-test/--mouse-test und frischem separatem Verzeichnis. Die echte EXE und tatsächliche Ausgabedateien prüfen. Persönliche Freigabe und technischer Test sind getrennt.

Neue Funktionen nur aus aktuellem Auftrag. Arbeitsname bleibt vorläufig. Keine Website oder Remote-Veröffentlichung aus lokaler Lieferung ableiten.
