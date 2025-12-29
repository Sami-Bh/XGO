# .github/workflows
contains workflows deployment to azure

# client
contains react app using :
- mui
- react rooter
- MSAL
- Zod
- react query

#IAC
- layer 00 contains terraform's infrastructure 
- layer 01 contains applications' infrasturecture
#XGO.Store
is the api that handles products and categories

#XGO.Storage
is the api that handles products' storage

#XGO.ApiGateway
hosts the react app and servers as a reverse proxy 