# ApplicationUpdater
ApplicationUpdater (to update manualy application on IIS server)

### Szybki start

Aplikacja s³u¿y do aktualizacji folderów i wykonywania dodatkowych czynnoœæi podczas tego procesu. Na przyk³ad: zanim zrobi aktualizacjê folderu najpierw sprawdzi czy pliki podmieniane s¹ w nowszej wersji ni¿ te, które maj¹ zostaæ podmienione, zrobi automatyczny backup folderu itd. Sczegó³y poni¿ej. Aplikacja jest przetestowana i dobrze wygrzana na ponad 20 serwerach. Aplikacji nie potrzeba instalowaæ, jest to "wszystko zawieraj¹ca paczka". Zawiera .NET Cora 3.0 i aplikacjê. 

### Parametry wywo³ania


ApplicationUpdater to konsolowa aplikacja, któr¹ wywo³uje siê podaj¹c szereg paametrów wywo³ania. Zalecanty sposób obs³ugi to utworzyæ plik *.bat, który bedzie nazywa³ siê np: update-prod.bat i bêdzie zawiera³ parametry wywo³ania. Generalnie parametry per serwer s¹ sta³e.

Parametry(wartoœci) s¹ podawane po spacji. Œcie¿ki mogê byæ w cudzys³owiach.

| Nazwa parametru  | Czy obowi¹zkowy?  |  Wartoœci  | Opis  |
|---|---|---|---|
|  Strategy |  Tak | Selgros,Orlen   | Pojedyñcze strategie, które ró¿ni¹ siê kolejnoœæi¹ wykonywanych opercji jak i samymi operacjami.  | 
| PathToZipFile  |Tak  | "d:/aapap.zip"   |Œcie¿ka do pliku zip z folderem do podmiany   |   
| BackupDirectory  | Tak  | "d:/Backup"  | Œcie¿ka do folderu, gdzie bêd¹ umieszczone kopie zapasowe  |   
| IntepubDirectory  | Tak  | "d:/Intepub/wwwwroot"  | Œcie¿ka do folderu, gdzie pliki bêd¹ podmieniane  |   
| Version  | Tak  | "99.543"  | Identyfikator wersji, który zostanie umieszczony w web.config  |   
| IsUndoProcess  | Tak  | true,false  | Prze³¹cznik informuj¹cy czy wykonaæ proces odwrotny czyli odzwyskanie z kopi zapasowej.  |   


### Zaimplementowane kroki

Aplikacja mo¿e wykoaæ klikanaœcie czynnoœæi. S¹ to: stworzenie folderu kopi zapasowej, wypakowanie do niego pliku *.zip, porównanie plików wypakowanych z nadpisywanymi, wykonanie kopi zapasowej œcie¿ki docelowej(InetpubDirecotry), wykonanie kopiowania, podmiana klucza w web.config-u.

### Opis króków

cdn.


