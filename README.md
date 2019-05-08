# Token Magic
A project to test out Json Web Tokens and discoverablility of apis.

## Getting started development
* Install Auth/dummyCert.pfx as local user

### If you dont know the password to dummyCert.pfx 
* generate your own cert 
  * open powershell (probably have to be admin)
  * `$cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname localhost`
    * you can probably name the cert (localhost above) whatever you want
  * open 'mmc' from the windows run window (windows key + R)
  * add the certificates snap in
  * select local machine
  * find the cert you just created and right click -> export
  * save the cert however you like
    * probably should add a password
  * double click the cert file, install as local user
    * can probably save time by modifying the above powershell command to create local user cert from the begining...
  * copy the cert's thumbprint into the app
