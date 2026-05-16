FROM node:20-slim

WORKDIR /app

# Copiamos primero los archivos de dependencias para aprovechar la caché de Docker
COPY package.json package-lock.json* ./
RUN npm install

# Copiamos el resto del código
COPY . .

# Exponemos el puerto por defecto de Vite
EXPOSE 5173

# El parámetro --host es CRÍTICO. Sin él, Vite solo se sirve a sí mismo 
# dentro del contenedor y no podrías verlo desde tu navegador web.
CMD ["npm", "run", "dev", "--", "--host"]