# Usamos una imagen de Python oficial muy ligera
FROM python:3.9-slim

# Creamos una carpeta dentro del contenedor para el código
WORKDIR /app

# Por ahora, para que no falle si no tienes el .py, 
# le decimos que se quede encendido esperando
CMD ["python", "-c", "import time; print('Contenedor listo y esperando código...'); [time.sleep(60) for _ in range(1000)]"]
