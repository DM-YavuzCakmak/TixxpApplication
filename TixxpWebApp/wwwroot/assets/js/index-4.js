var options = {
    series: [{
    name: 'Website Blog',
    type: 'column',
    data: [440, 505, 414, 671, 227, 413, 201, 352, 752, 320, 257, 160]
  }, {
    name: 'Social Media',
    type: 'line',
    data: [23, 42, 35, 27, 43, 22, 17, 31, 22, 22, 12, 16]
  }],
    chart: {
    height: 360,
    type: 'line',
  },
  plotOptions: {
      bar: {
          horizontal: false,
          columnWidth:'25%',
          borderRadius: 1,
      }
  },
  stroke: {
    width: [0, 4]
  },
  colors:["#6f40a5","#f66b4e"],
  title: {
    text: 'Traffic Sources'
  },
  dataLabels: {
    enabled: true,
    enabledOnSeries: [1]
  },
  labels: ['01 Jan 2001', '02 Jan 2001', '03 Jan 2001', '04 Jan 2001', '05 Jan 2001', '06 Jan 2001', '07 Jan 2001', '08 Jan 2001', '09 Jan 2001', '10 Jan 2001', '11 Jan 2001', '12 Jan 2001'],
  xaxis: {
    type: 'datetime'
  },
  yaxis: [{
    title: {
      text: 'Website Blog',
    },
  
  }, {
    opposite: true,
    title: {
      text: 'Social Media'
    }
  }]
  };

  var chart1 = new ApexCharts(document.querySelector("#chart"), options);
  chart1.render();

  function charts() {
    chart1.updateOptions({
      colors: ["rgb(" + myVarVal + ")","#f66b4e" ],
    });
  }

  /* Visitors By Country Map */
  var markers = [
    {
      name: "Usa",
      coords: [40.3, -101.38],
    },
    {
      name: "India",
      coords: [20.5937, 78.9629],
    },
    {
      name: "Vatican City",
      coords: [41.9, 12.45],
    },
    {
      name: "Canada",
      coords: [56.1304, -106.3468],
    },
    {
      name: "Mauritius",
      coords: [-20.2, 57.5],
    },
    {
      name: "Singapore",
      coords: [1.3, 103.8],
    },
    {
      name: "Palau",
      coords: [7.35, 134.46],
    },
    {
      name: "Maldives",
      coords: [3.2, 73.22],
    },
    {
      name: "São Tomé and Príncipe",
      coords: [0.33, 6.73],
    },
  ];
  var map = new jsVectorMap({
    map: "world_merc",
    selector: "#visitors-countries",
    markersSelectable: true,
    zoomOnScroll: false,
    zoomButtons: false,
  
    onMarkerSelected(index, isSelected, selectedMarkers) {
      console.log(index, isSelected, selectedMarkers);
    },
  
    // -------- Labels --------
    labels: {
      markers: {
        render: function (marker) {
          return marker.name;
        },
      },
    },
  
    // -------- Marker and label style --------
    markers: markers,
    markerStyle: {
      hover: {
        stroke: "#DDD",
        strokeWidth: 3,
        fill: "#FFF",
      },
      selected: {
        fill: "#ff525d",
      },
    },
    markerLabelStyle: {
      initial: {
        fontFamily: "Poppins",
        fontSize: 13,
        fontWeight: 500,
        fill: "#35373e",
      },
    },
  });
  /* Visitors By Country Map */
