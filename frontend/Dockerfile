FROM node:16 AS builder
ARG VITE_MAPBOX_API_KEY
ENV VITE_MAPBOX_API_KEY $VITE_MAPBOX_API_KEY
ARG VITE_API_URL
ENV VITE_API_URL $VITE_API_URL
ARG VITE_VK_CLIENT_ID
ENV VITE_VK_CLIENT_ID $VITE_VK_CLIENT_ID
ARG VITE_REDIRECT_URL
ENV VITE_REDIRECT_URL $VITE_REDIRECT_URL
ARG VITE_VK_AUTH_URL
ENV VITE_VK_AUTH_URL $VITE_VK_AUTH_URL
WORKDIR /app
COPY onied/package.json onied/package-lock.json ./
RUN npm ci

COPY onied .
RUN npm run build

FROM nginx:stable-alpine
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080
CMD ["nginx", "-g", "daemon off;"]
