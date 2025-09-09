# Build stage
FROM node:20 AS build
WORKDIR /app

# Copiar package.json e package-lock.json e instalar dependências
COPY package*.json ./
RUN npm install

# Copiar todo o código fonte
COPY . .

# Declarar ARG para build
ARG REACT_APP_BACKEND_URL
ENV REACT_APP_BACKEND_URL=$REACT_APP_BACKEND_URL

# Build do React
RUN npm run build

# Nginx stage
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copiar build do React para Nginx
COPY --from=build /app/build .

# Expor porta 80
EXPOSE 80

# Rodar Nginx em primeiro plano
CMD ["nginx", "-g", "daemon off;"]
