## pruhk-ppm

This is where you include your WebPart documentation.

### Set up your development environment

```bash
npm install -g yo gulp
npm install -g @microsoft/generator-sharepoint
npm install @microsoft/generator-sharepoint --save-dev
```

### Building the code

```bash
git clone the repo
npm i
npm i -g gulp
gulp
```

This package produces the following:

* lib/* - intermediate-stage commonjs build artifacts
* dist/* - the bundled script, along with other resources
* deploy/* - all resources which should be uploaded to a CDN.

### Build options

gulp clean - TODO
gulp test - TODO
gulp serve - TODO
gulp bundle - TODO
gulp package-solution - TODO

### Deploy

```powershell
.\VSTS.DeploySPFxToAppCatalog.ps1 -username admin@makermct.onmicrosoft.com -psw '********' -catalogSite https://makermct.sharepoint.com/sites/apps -releaseFolder '\'
```