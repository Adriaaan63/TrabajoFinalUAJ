FROM python:3.11-slim

# Buenas prácticas estándar de Python en Docker:
#  · DONTWRITEBYTECODE evita .pyc innecesarios en el contenedor.
#  · UNBUFFERED hace que los logs salgan inmediatamente (no se quedan en
#    buffer de stdout, importante para `docker compose logs -f`).
ENV PYTHONDONTWRITEBYTECODE=1 \
    PYTHONUNBUFFERED=1 \
    PIP_NO_CACHE_DIR=1

WORKDIR /app

# Capa de dependencias separada del código: si solo cambia código y no
# requirements.txt, Docker reutiliza esta capa cacheada.
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Código de la aplicación
COPY . .

# Ejecutamos como usuario no privilegiado: si la API se compromete,
# el atacante no tiene root en el contenedor.
RUN useradd --create-home --shell /bin/bash queryapi \
    && chown -R queryapi:queryapi /app
USER queryapi

EXPOSE 8001

# Docker comprueba periódicamente que la API responde. Si /health
# devuelve != 200, marca el contenedor como unhealthy.
HEALTHCHECK --interval=30s --timeout=5s --start-period=15s --retries=3 \
    CMD python -c "import urllib.request,sys; \
sys.exit(0 if urllib.request.urlopen('http://localhost:8001/health',timeout=3).getcode()==200 else 1)" \
    || exit 1

# uvicorn directo es más idiomático que `python main.py` en producción
# (mejor manejo de señales, --workers, etc). En docker-compose se puede
# sobreescribir con `command:` si se prefiere.
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8001"]