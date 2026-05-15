FROM python:3.11-slim

ENV PYTHONDONTWRITEBYTECODE=1 \
    PYTHONUNBUFFERED=1 \
    PIP_NO_CACHE_DIR=1

WORKDIR /app
COPY requirements_worker.txt ./requirements.txt
RUN pip install --no-cache-dir -r requirements.txt
COPY worker.py .

RUN useradd --create-home --shell /bin/bash metricsworker \
    && chown -R metricsworker:metricsworker /app
USER metricsworker

CMD ["python", "worker.py"]
