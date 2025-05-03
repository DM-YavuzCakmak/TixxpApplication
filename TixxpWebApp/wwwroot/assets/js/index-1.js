/*  sales overview chart */
var options = {
    series: [
      {
        name: "Amount",
        data: [44, 42, 57, 86, 58, 55, 70, 43, 23, 54, 77, 34],
      },
      {
        name: "Total Budget",
        data: [74, 72, 87, 116, 88, 85, 100, 73, 53, 84, 107, 64],
      }
    ],
    chart: {
      stacked: true,
      type: "bar",
      height: 325,
    },
    grid: {
      borderColor: "#f5f4f4",
      strokeDashArray: 5,
      yaxis: {
        lines: {
          show: true, // Ensure y-axis grids are shown
        },
      },
    },
    colors: ["#623aa2", "#fc9e52",],
    plotOptions: {
      bar: {
        colors: {
          ranges: [
            {
              from: -100,
              to: -46,
              color: "#ebeff5",
            },
            {
              from: -45,
              to: 0,
              color: "#ebeff5",
            },
          ],
        },
        columnWidth: "30%",
      },
    },
    dataLabels: {
      enabled: false,
    },
    legend: {
      show: true,
      position: "top",
    },
    yaxis: {
      title: {
        text: "Growth",
        style: {
          color: "#adb5be",
          fontSize: "14px",
          fontFamily: "Montserrat, sans-serif",
          fontWeight: 600,
          cssClass: "apexcharts-yaxis-label",
        },
      },
      axisBorder: {
        show: true,
        color: "rgba(119, 119, 142, 0.05)",
        offsetX: 0,
        offsetY: 0,
      },
      axisTicks: {
        show: true,
        borderType: "solid",
        color: "rgba(119, 119, 142, 0.05)",
        width: 6,
        offsetX: 0,
        offsetY: 0,
      },
      labels: {
        formatter: function (y) {
          return y.toFixed(0) + "";
        },
      },
    },
    xaxis: {
      type: "month",
      categories: [
        "Jan",
        "Feb",
        "Mar",
        "Apr",
        "May",
        "Jun",
        "Jul",
        "Aug",
        "sep",
        "oct",
        "nov",
        "dec",
      ],
      axisBorder: {
        show: false,
        color: "rgba(119, 119, 142, 0.05)",
        offsetX: 0,
        offsetY: 0,
      },
      axisTicks: {
        show: false,
        borderType: "solid",
        color: "rgba(119, 119, 142, 0.05)",
        width: 6,
        offsetX: 0,
        offsetY: 0,
      },
      labels: {
        rotate: -90,
      },
    },
  };
  document.getElementById("salesOverview").innerHTML = "";
  var chart = new ApexCharts(document.querySelector("#salesOverview"), options);
  chart.render();
  function salesOverview() {
    chart.updateOptions({
      colors: ["rgb(" + myVarVal + ")","#fc9e52",],
    });
  }
  /*  sales overview chart */
/* sale value chart */
var options = {
    chart: {
      height: 275,
      type: "radialBar",
    },
  
    series: [60],
    colors: ["rgb(132, 90, 223)"],
    plotOptions: {
      radialBar: {
        hollow: {
          margin: 0,
          size: "70%",
          background: "#fff",
        },
        track: {
          dropShadow: {
            enabled: true,
            top: 2,
            left: 0,
            blur: 2,
            opacity: 0.15,
          },
        },
        dataLabels: {
          name: {
            offsetY: -10,
            color: "#4b9bfa",
            fontSize: "16px",
            show: false,
          },
          value: {
            color: "#4b9bfa",
            fontSize: "30px",
            show: true,
          },
        },
      },
    },
    stroke: {
      lineCap: "square",
    },
    labels: ["Cart"],
  };
  document.querySelector("#average-sales").innerHTML = "";
  var chart1 = new ApexCharts(document.querySelector("#average-sales"), options);
  chart1.render();
  
  function averagesales() {
    chart1.updateOptions({
      colors: ["rgb(" + myVarVal + ")"],
    });
  }
  /* sale value chart */