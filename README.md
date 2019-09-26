# ApplicationUpdater
ApplicationUpdater (to update manualy application on IIS server)

### Szybki start

Aplikacja s�u�y do aktualizacji folder�w i wykonywania dodatkowych czynno��i podczas tego procesu. Na przyk�ad: zanim zrobi aktualizacj� folderu najpierw sprawdzi czy pliki podmieniane s� w nowszej wersji ni� te, kt�re maj� zosta� podmienione, zrobi automatyczny backup folderu itd. Sczeg�y poni�ej. Aplikacja jest przetestowana i dobrze wygrzana na ponad 20 serwerach. Aplikacji nie potrzeba instalowa�, jest to "wszystko zawieraj�ca paczka". Zawiera .NET Cora 3.0 i aplikacj�. 

### Parametry wywo�ania


ApplicationUpdater to konsolowa aplikacja, kt�r� wywo�uje si� podaj�c szereg paametr�w wywo�ania. Zalecanty spos�b obs�ugi to utworzy� plik *.bat, kt�ry bedzie nazywa� si� np: update-prod.bat i b�dzie zawiera� parametry wywo�ania. Generalnie parametry per serwer s� sta�e.

Parametry(warto�ci) s� podawane po spacji. �cie�ki mog� by� w cudzys�owiach.

| Nazwa parametru  | Czy obowi�zkowy?  |  Warto�ci  | Opis  |
|---|---|---|---|
|  Strategy |  Tak | Selgros,Orlen   | Pojedy�cze strategie, kt�re r�ni� si� kolejno��i� wykonywanych opercji jak i samymi operacjami.  | 
| PathToZipFile  |Tak  | "d:/aapap.zip"   |�cie�ka do pliku zip z folderem do podmiany   |   
| BackupDirectory  | Tak  | "d:/Backup"  | �cie�ka do folderu, gdzie b�d� umieszczone kopie zapasowe  |   
| IntepubDirectory  | Tak  | "d:/Intepub/wwwwroot"  | �cie�ka do folderu, gdzie pliki b�d� podmieniane  |   
| Version  | Tak  | "99.543"  | Identyfikator wersji, kt�ry zostanie umieszczony w web.config  |   
| IsUndoProcess  | Tak  | true,false  | Prze��cznik informuj�cy czy wykona� proces odwrotny czyli odzwyskanie z kopi zapasowej.  |   


### Zaimplementowane kroki

Aplikacja mo�e wykoa� klikana�cie czynno��i. S� to: stworzenie folderu kopi zapasowej, wypakowanie do niego pliku *.zip, por�wnanie plik�w wypakowanych z nadpisywanymi, wykonanie kopi zapasowej �cie�ki docelowej(InetpubDirecotry), wykonanie kopiowania, podmiana klucza w web.config-u.

### Opis kr�k�w

cdn.


