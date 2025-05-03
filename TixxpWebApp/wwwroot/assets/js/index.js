/* Bitcoin Chart */
  var chart1 = {
      series: [{
          name: 'Income',
          data: [130, 90, 108,80, 136, 92, 149, 93, 113, 88, 93, 108],
  }, {
    name: 'Expances',
    data: [100, 85, 128, 100, 80, 125, 105, 118, 90, 145, 105, 125]
  }],
      chart: {
          type: 'area',
          height: 340
      },
      grid: {
          borderColor: 'rgba(167, 180, 201 ,0.2)',
      },
  markers: {
    size: [0,0],
    strokeColors: '#fff',
    strokeWidth: [3, 3],
    strokeOpacity: 0,
  },
  stroke: {
    curve: 'smooth',
    width: 2,
    dashArray: [0, 0]
  },
  fill: {
  	type: ['gradient','gradient'],
  	gradient: {
  		shade: 'light',
  		type: "vertical",
  		opacityFrom: [0.6, 0.5],
  		opacityTo: [0.2, 0.1],
  		stops: [0, 100]
  	}
  },
      dataLabels: {
          enabled: false,
      },
      legend: {
          show: true,
    position: 'top',
          labels: {
              colors: '#74767c',
          },
      },
      yaxis: {
          labels: {
              formatter: function (y) {
                  return y.toFixed(0) + "";
              }
          },
          labels: {
              style: {
                  colors: "#8c9097",
                  fontSize: '11px',
                  fontWeight: 600,
                  cssClass: 'apexcharts-xaxis-label',
              },
          }
      },
      xaxis: {
          type: 'month',
          categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'sep', 'oct', 'nov', 'dec'],
          axisBorder: {
              show: true,
              color: 'rgba(167, 180, 201 ,0.2)',
              offsetX: 0,
              offsetY: 0,
          },
          axisTicks: {
              show: true,
              borderType: 'solid',
              color: 'rgba(167, 180, 201 ,0.2)',
              width: 6,
              offsetX: 0,
              offsetY: 0
          },
          labels: {
              rotate: -90,
              style: {
                  colors: "#8c9097",
                  fontSize: '11px',
                  fontWeight: 600,
                  cssClass: 'apexcharts-xaxis-label',
              },
          }
      },
      
    colors: [  "#f66b4e",  "#623aa2"],
  };
  document.getElementById('balance').innerHTML = '';
  var chart1 = new ApexCharts(document.querySelector("#balance"), chart1);
  chart1.render();

function balance() {
	chart1.updateOptions({
    colors: [ "#f66b4e", "rgba(" + myVarVal + ", 0.95)"],
	})
}