-- read me voor eID smart card reader ---

Gevonden documentatie en links
-------------------------------

Test omgeving : https://github.com/Fedict/eid-test-cards
Middleware omgeving: https://github.com/Fedict/eid-mw
Documentatie eID viewer (C en C# wrapper ) ( en module API) : https://dist.eid.belgium.be/releases/eidviewer/
Main website: https://eid.belgium.be/en
Certificate test : https://test.eid.belgium.be/
WebApplicatie: https://www.e-contract.be/developers/webapp

Documentatie voor SDK
----------------------
eid-mw -> doc -> sdk -> documentation

Aangeraden om git repo's niet te clonen , aangezien issues op de master branch dan geen invloed hebben op dit project

Huidige vragen / pitfall's
--------------------------

Verschillen tussen PKCS (public key cryptography standards)?
* Waarmee werkt eid-mw? (PKCS 11) cryptoki
* Test omgeving is afhankelijk van 'mock' dll om hardware reader te omzeilen. Kan niet gevonden of gecompiled worden. (winscard.dll)
* 

Mogelelijke procedure om een kaart uit te lezen
*********************************
Normaal gezien worden sommige (java) applets gebruikt om informatie op te halen, maar die zijn niet beschikbaar meer/ overbodig geworden
Eventueel kan de 'eID viewer' gebruikt worden om informatie op te halen en op te slaan (in xml) en zo op te halen met de webapplicatie (geen geïntegreerde oplossing)
De documentatie die voor de api die in C geschreven werd voor de Cryptoki API ( pkcs-11v2-11r1) zou normaal gezien ook 
van toepassing moeten zijn op de wrapper in C# (project Pkcs11Net). 
Volgens e-contract.be: 
"...Indien eID-beveiliging niet je core business is, laat dergelijke home-made integraties links liggen..." en
"...erder is een technologie zoals eID Applet tegenwoordig niet meer toereikend om alle platformen te bedienen..."

Aanbevolen voor ASP.net integratie (niet .net mvc): eID identity provider => DotNetOpenAuth by DotNetOpenAuth (deprecated)
Andere identity providers : OpenID?


Cryptoki (PKCS11 API nota's)
----------------------------
Belangrijkste hoofdstukken (2:scope & 6:Overview)
Sessions, R/O & R/W sessions mogelijk afhankelijk van type gebruiker in sessie (zie state diagrams).
Attributes=> Create, Edit, Search naar objecten. objecten enkel toegankelijk in een sessie (moet zelf gestart worden);
Verschillende sessiemogelijkheden en limitaties uitgelegd met een use case. (Toegankelijkheid en overervering van sessies bij SO/user).
Getting started (pcks wrapper) voorlopig nog .ddl dependency op te lossens(64 bit & 32 bit )
De standaard library die interfaced met de cardreader (gebaseerd op pcks11) heet beidpkcs11.dll en bevindt zich in de system32 folder.

LINKS
^^^^^^
	*General Model: https://www.cryptsoft.com/pkcs11doc/v220/group__SEC__6__3__GENERAL__MODEL.html
		Tags: Tokens, Slots, OverView
	*OASIS standard online reference: http://docs.oasis-open.org/pkcs11/pkcs11-base/v2.40/os/pkcs11-base-v2.40-os.html
		Tags: Object hierarchy, Prefix descriptions. 
	*Creating a Wrapper for C++ API : https://docs.microsoft.com/en-us/dotnet/framework/interop/index
		*API documentatie voor wrapper : https://pkcs11interop.net/doc/
		Tags: Runtime.interop, unmanaged libraries, COM
	*External finished Wrapper for C++ api : https://pkcs11interop.net/
		Tags: Nuget package, 3d party, Support for BeID
	*webUSB: https://developers.google.com/web/updates/2016/03/access-usb-devices-on-the-web
		Tags: web integratie, peripheral devices, web service?


		