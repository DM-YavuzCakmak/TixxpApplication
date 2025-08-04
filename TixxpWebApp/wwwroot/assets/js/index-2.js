/* Total-orders Chart */
var options = {
    chart: {
        height: 130,
        width: 100,
        type: "radialBar",
    },

    series: [68],
    colors: ["#b50d05"],
    plotOptions: {
        radialBar: {
            hollow: {
                margin: 0,
                size: "50%",
                background: "#fff"
            },
            dataLabels: {
                name: {
                    offsetY: -10,
                    color: "#4b9bfa",
                    fontSize: ".625rem",
                    show: false
                },
                value: {
                    offsetY: 5,
                    color: "#4b9bfa",
                    fontSize: ".875rem",
                    show: true,
                    fontWeight: 600
                }
            }
        }
    },
    stroke: {
        lineCap: "round"
    },
    labels: ["Status"]
};
document.querySelector("#total-orders").innerHTML = ""
var chart = new ApexCharts(document.querySelector("#total-orders"), options);
chart.render();
/* Total-orders Chart */

/* Total-views Chart */
var options = {
    chart: {
        height: 130,
        width: 100,
        type: "radialBar",
    },

    series: [68],
    colors: ["#0a7326"],
    plotOptions: {
        radialBar: {
            hollow: {
                margin: 0,
                size: "50%",
                background: "#fff"
            },
            dataLabels: {
                name: {
                    offsetY: -10,
                    color: "#4b9bfa",
                    fontSize: ".625rem",
                    show: false
                },
                value: {
                    offsetY: 5,
                    color: "#4b9bfa",
                    fontSize: ".875rem",
                    show: true,
                    fontWeight: 600
                }
            }
        }
    },
    stroke: {
        lineCap: "round"
    },
    labels: ["Status"]
};
document.querySelector("#total-views").innerHTML = ""
var chart = new ApexCharts(document.querySelector("#total-views"), options);
chart.render();
/* Total-views Chart */

/* Total-earnings Chart */
var options = {
    chart: {
        height: 130,
        width: 100,
        type: "radialBar",
    },

    series: [68],
    colors: ["#da4a25"],
    plotOptions: {
        radialBar: {
            hollow: {
                margin: 0,
                size: "50%",
                background: "#fff"
            },
            dataLabels: {
                name: {
                    offsetY: -10,
                    color: "#4b9bfa",
                    fontSize: ".625rem",
                    show: false
                },
                value: {
                    offsetY: 5,
                    color: "#4b9bfa",
                    fontSize: ".875rem",
                    show: true,
                    fontWeight: 600
                }
            }
        }
    },
    stroke: {
        lineCap: "round"
    },
    labels: ["Status"]
};
document.querySelector("#total-earnings").innerHTML = ""
var chart = new ApexCharts(document.querySelector("#total-earnings"), options);
chart.render();
/* Total-earnings Chart */

/* Total-comments Chart */
var options = {
    chart: {
        height: 130,
        width: 100,
        type: "radialBar",
    },

    series: [68],
    colors: ["#3578d5"],
    plotOptions: {
        radialBar: {
            hollow: {
                margin: 0,
                size: "50%",
                background: "#fff"
            },
            dataLabels: {
                name: {
                    offsetY: -10,
                    color: "#4b9bfa",
                    fontSize: ".625rem",
                    show: false
                },
                value: {
                    offsetY: 5,
                    color: "#4b9bfa",
                    fontSize: ".875rem",
                    show: true,
                    fontWeight: 600
                }
            }
        }
    },
    stroke: {
        lineCap: "round"
    },
    labels: ["Status"]
};
document.querySelector("#total-comments").innerHTML = ""
var chart = new ApexCharts(document.querySelector("#total-comments"), options);
chart.render();
/* Total-comments Chart */

// for saless Statistics
var options = {
	series: [{
		name: "Number Of Ticket",
		data: [20, 58, 38, 72, 55, 63, 43, 76, 55, 80, 40, 80]
	}, {
		name: "Amount",
		data: [65, 45, 75, 38, 85, 35, 62, 40, 40, 64, 50, 89]
	}],
	chart: {
		height: 340,
		type: 'line',
		zoom: {
			enabled: false
		},
		dropShadow: {
			enabled: true,
			enabledOnSeries: undefined,
			top: 5,
			left: 0,
			blur: 3,
			color: '#000',
			opacity: 0.1
		},
	},
	dataLabels: {
		enabled: false
	},
	legend: {
		position: "top",
		horizontalAlign: "center",
		offsetX: -15,
		fontWeight: "bold",
	},
	stroke: {
		curve: 'smooth',
		width: '3',
		dashArray: [0, 0],
	},
	grid: {
		borderColor: '#f2f6f7',
	},
	colors: ["rgb(132, 90, 223)", "#fc9e52"],
	yaxis: {
		title: {
			text: 'Statistics',
			style: {
				color: '#adb5be',
				fontSize: '14px',
				fontFamily: 'poppins, sans-serif',
				fontWeight: 600,
				cssClass: 'apexcharts-yaxis-label',
			},
		},
	},
	xaxis: {
		type: 'month',
		categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
		axisBorder: {
			show: true,
			color: 'rgba(119, 119, 142, 0.05)',
			offsetX: 0,
			offsetY: 0,
		},
		axisTicks: {
			show: true,
			borderType: 'solid',
			color: 'rgba(119, 119, 142, 0.05)',
			width: 6,
			offsetX: 0,
			offsetY: 0
		},
		labels: {
			rotate: -90
		}
	}
};
document.getElementById('sales-statistics').innerHTML = ''
var chart = new ApexCharts(document.querySelector("#sales-statistics"), options);
chart.render();
function salesStatistics() {
	chart.updateOptions({
		colors: ["rgb(" + myVarVal + ")", "#fc9e52"],
	})
}
// Leads By Source Chart
var options = {
    series: [{
        name: 'Number Of Reservation',
        data: [450, 780, 550, 940, 1100, 1200, 1380,500,730, 800, 1000,402,500]
    }],
    chart: {
        fontFamily: 'Poppins, Arial, sans-serif',
        toolbar: {
            show: false
        },
        type: 'bar',
        height: 320
    },
    grid: {
        borderColor: '#f2f6f7',
    },
    plotOptions: {
        bar: {
            horizontal: false,
            barHeight: "30%",
            borderRadius: 1,
        }
    },
    colors: ["#f59032"],
    dataLabels: {
        enabled: false
    },
    xaxis: {
        categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    }
};
var chart2 = new ApexCharts(document.querySelector("#lineChart1"), options);
chart2.render();

function lineChart1() {
    chart2.updateOptions({
        colors: ["rgba(" + myVarVal + ", 0.95)"],
    })
}