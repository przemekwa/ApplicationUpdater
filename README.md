# ApplicationUpdater
ApplicationUpdater (to update manualy application on IIS server)

### Szybki start

Aplikacja służy do aktualizacji folderów i wykonywania dodatkowych czynnośći podczas tego procesu. Na przykład: zanim zrobi aktualizację folderu najpierw sprawdzi czy pliki podmieniane są w nowszej wersji niż te, które mają zostać podmienione, zrobi automatyczny backup folderu itd. Sczegóły poniżej. Aplikacja jest przetestowana i dobrze wygrzana na ponad 20 serwerach. Aplikacji nie potrzeba instalować, jest to "wszystko zawierająca paczka". Zawiera .NET Cora 3.0 i aplikację. 

### Parametry wywołania


ApplicationUpdater to konsolowa aplikacja, którą wywołuje się podając szereg paametrów wywołania. Zalecanty sposób obsługi to utworzyć plik *.bat, który bedzie nazywał się np: update-prod.bat i będzie zawierał parametry wywołania. Generalnie parametry per serwer są stałe.

Parametry(wartości) są podawane po spacji. Ścieżki mogę być w cudzysłowiach.

| Nazwa parametru  | Czy obowiązkowy?  |  Wartości  | Opis  |
|---|---|---|---|
|  Strategy |  Tak | Selgros,Orlen,MiniRelease   | Pojedyńcze strategie, które różnią się kolejnośćią wykonywanych opercji jak i samymi operacjami.  | 
| PathToZipFile  |Tak  | "d:/aapap.zip"   |Ścieżka do pliku zip z folderem do podmiany   |   
| BackupDirectory  | Tak  | "d:/Backup"  | Ścieżka do folderu, gdzie będą umieszczone kopie zapasowe  |   
| IntepubDirectory  | Tak  | "d:/Intepub/wwwwroot"  | Ścieżka do folderu, gdzie pliki będą podmieniane  |   
| Version  | Tak  | "99.543"  | Identyfikator wersji, który zostanie umieszczony w web.config  |   
| IsUndoProcess  | Tak  | true,false  | Przełącznik informujący czy wykonać proces odwrotny czyli odzwyskanie z kopi zapasowej. Zostanie wykonana operacja przywrócenia ostatniej produkcyjnej wersji. |   


### Zaimplementowane kroki

Aplikacja może wykoać klikanaście czynnośći. Są to: stworzenie folderu kopi zapasowej, wypakowanie do niego pliku *.zip, porównanie plików wypakowanych z nadpisywanymi, wykonanie kopi zapasowej ścieżki docelowej(InetpubDirecotry), wykonanie kopiowania, podmiana klucza w web.config-u.

### Opis króków

cdn.


