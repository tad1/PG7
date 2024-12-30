#

wykorzystanie własnej dyrektywy: v-focus
przynajmniej jednej funkcji wielokrotnego użytku (Composable): useSnackbar
2 dodatkowe wtyczki: vuetify (i.e. VExpansionPanel) + axios
Pinia: src/store/authInfo.ts
własna wtyczka: src/plugins/authInfo.ts

run app:
    docker run --name mongodb -p 27017:27017 -d mongodb/mongodb-community-server:latest
    npm run dev
run unit tests:
    npm run test:unit
run cypress:
    npm run test:e2e:dev
