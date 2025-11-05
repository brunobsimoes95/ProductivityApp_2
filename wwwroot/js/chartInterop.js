let sleepChartInstance = null;

// Inicia os efeitos visuais ao carregar
function initChartEffects() {
    document.querySelectorAll('.emotions-card, .sleep-chart-container, .chart-container, .no-data-message').forEach(el => {
        el.classList.add('animate__animated', 'animate__fadeInUp');
    });

    const emotionItems = document.querySelectorAll('.emotion-item');
    emotionItems.forEach((item, index) => {
        item.style.opacity = 0;
        item.style.transform = 'translateY(20px)';

        setTimeout(() => {
            item.style.transition = 'all 0.5s ease';
            item.style.opacity = 1;
            item.style.transform = 'translateY(0)';
        }, 100 + (index * 100));
    });
}

// Anima barras do gráfico
function animateChartBars() {
    const bars = document.querySelectorAll('.chart-bar');
    bars.forEach((bar, index) => {
        const originalHeight = bar.style.height;
        bar.style.height = '0px';

        setTimeout(() => {
            bar.style.transition = 'height 0.8s cubic-bezier(0.34, 1.56, 0.64, 1)';
            bar.style.height = originalHeight;
        }, 300 + (index * 100));
    });
}

// Mostra detalhes do dia num modal
function showDayDetails(details, day, emoji) {
    const modal = document.createElement('div');
    modal.className = 'custom-modal';
    modal.style = `position: fixed; top: 0; left: 0; width: 100%; height: 100%;
                   background-color: rgba(0,0,0,0.7); display: flex; justify-content: center; align-items: center;
                   z-index: 9999; opacity: 0; transition: opacity 0.3s ease;`;

    const modalContent = document.createElement('div');
    modalContent.className = 'modal-content';
    modalContent.style = `background-color: white; border-radius: 16px; padding: 25px;
                          max-width: 400px; width: 80%; box-shadow: 0 10px 25px rgba(0,0,0,0.5);
                          transform: translateY(20px); transition: transform 0.3s ease;`;

    const title = document.createElement('h3');
    title.innerHTML = `${emoji} Detalhes do Dia: ${day}`;
    title.style = `margin-bottom: 15px; text-align: center; color: #333;
                   border-bottom: 2px solid #3db9dc; padding-bottom: 10px;`;

    const content = document.createElement('pre');
    content.textContent = details;
    content.style = `white-space: pre-wrap; margin: 15px 0; padding: 10px;
                     background-color: #f5f5f5; border-radius: 8px; font-size: 14px;`;

    const closeBtn = document.createElement('button');
    closeBtn.textContent = 'Fechar';
    closeBtn.style = `display: block; margin: 10px auto 0; padding: 8px 16px;
                      background-color: #3db9dc; color: white; border: none; border-radius: 20px; cursor: pointer;`;

    closeBtn.addEventListener('mouseover', () => closeBtn.style.backgroundColor = '#2b92b8');
    closeBtn.addEventListener('mouseout', () => closeBtn.style.backgroundColor = '#3db9dc');
    closeBtn.addEventListener('click', () => {
        modalContent.style.transform = 'translateY(20px)';
        modal.style.opacity = '0';
        setTimeout(() => document.body.removeChild(modal), 300);
    });

    modalContent.appendChild(title);
    modalContent.appendChild(content);
    modalContent.appendChild(closeBtn);
    modal.appendChild(modalContent);
    document.body.appendChild(modal);

    setTimeout(() => {
        modal.style.opacity = '1';
        modalContent.style.transform = 'translateY(0)';
    }, 10);

    modal.addEventListener('click', (e) => {
        if (e.target === modal) {
            modalContent.style.transform = 'translateY(20px)';
            modal.style.opacity = '0';
            setTimeout(() => document.body.removeChild(modal), 300);
        }
    });
}

// Exporta gráfico como imagem
function downloadChartImage(containerId) {
    const container = document.getElementById(containerId);
    if (!container) return;

    html2canvas(container).then(canvas => {
        const link = document.createElement('a');
        link.download = 'produtividade-chart.png';
        link.href = canvas.toDataURL('image/png');
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    });
}

// Adiciona hover ao container
function setupChartContainer(containerId) {
    const container = document.getElementById(containerId);
    if (!container) return;

    container.addEventListener('mouseenter', () => {
        container.style.transform = 'translateY(-5px)';
    });
    container.addEventListener('mouseleave', () => {
        container.style.transform = 'translateY(0)';
    });
}

// Anima botão de exportação
function animateExportButton() {
    const btn = document.querySelector('.btn-export:not(.processing)');
    if (!btn) return;

    btn.classList.add('animate__animated', 'animate__pulse');
    setTimeout(() => {
        btn.classList.remove('animate__animated', 'animate__pulse');
    }, 1000);
}

// Gráfico com scatter plot e linha de tendência
function renderImprovedScatterChart(canvasId, data, options) {
    console.log("✅ Versão corrigida da função foi carregada");

    const ctx = document.getElementById(canvasId).getContext('2d');
    if (!ctx || !data || data.length === 0) return;

    if (sleepChartInstance) sleepChartInstance.destroy();

    const xValues = data.map(d => d.x);
    const yValues = data.map(d => d.y);
    const n = data.length;
    const sumX = xValues.reduce((a, b) => a + b, 0);
    const sumY = yValues.reduce((a, b) => a + b, 0);
    const sumXY = xValues.reduce((sum, x, i) => sum + x * yValues[i], 0);
    const sumXX = xValues.reduce((sum, x) => sum + x * x, 0);

    const slope = (n * sumXY - sumX * sumY) / (n * sumXX - sumX * sumX);
    const intercept = (sumY - slope * sumX) / n;

    const minX = Math.min(...xValues);
    const maxX = Math.max(...xValues);

    const trendlineData = [
        { x: minX, y: minX * slope + intercept },
        { x: maxX, y: maxX * slope + intercept }
    ];

    sleepChartInstance = new Chart(ctx, {
        type: 'scatter',
        data: {
            datasets: [
                {
                    label: 'Sono vs Produtividade',
                    data: data,
                    pointBackgroundColor: options.pointBackgroundColor || '#3db9dc',
                    pointBorderColor: options.pointBorderColor || '#ffffff',
                    pointHoverBackgroundColor: options.pointHoverBackgroundColor || '#2b92b8',
                    pointHoverBorderColor: options.pointHoverBorderColor || '#ffffff',
                    pointRadius: options.pointRadius || 6,
                    pointHoverRadius: options.pointHoverRadius || 8,
                    backgroundColor: options.backgroundColor || 'rgba(61, 185, 220, 0.1)',
                    borderColor: options.borderColor || 'rgba(61, 185, 220, 0.8)',
                    showLine: false
                },
                {
                    label: 'Tendência',
                    data: trendlineData,
                    type: 'line',
                    pointRadius: 0,
                    borderColor: options.trendlineColor || 'rgba(255, 99, 132, 0.8)',
                    borderWidth: 2,
                    borderDash: [5, 5],
                    fill: false
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            animation: {
                duration: 1500,
                easing: 'easeOutQuart'
            },
            scales: {
                x: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Horas de Sono',
                        color: '#666',
                        font: { size: 14, weight: 'bold' }
                    },
                    ticks: { color: '#666' },
                    grid: { color: 'rgba(200, 200, 200, 0.2)', borderDash: [5, 5] }
                },
                y: {
                    beginAtZero: true,
                    max: 100,
                    title: {
                        display: true,
                        text: 'Produtividade',
                        color: '#666',
                        font: { size: 14, weight: 'bold' }
                    },
                    ticks: {
                        callback: function (value) { return value + '%'; },
                        color: '#666'
                    },
                    grid: { color: 'rgba(200, 200, 200, 0.2)', borderDash: [5, 5] }
                }
            },
            plugins: {
                legend: {
                    labels: {
                        color: '#666',
                        font: { size: 12 }
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(50, 50, 50, 0.8)',
                    callbacks: {
                        label: function (context) {
                            return `Sono: ${context.raw.x}h - Produtividade: ${context.raw.y}%`;
                        }
                    }
                }
            }
        }
    });
}
