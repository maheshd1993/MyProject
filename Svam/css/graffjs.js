$( document ).ready(function() {
    'use strict';

    var axisY = [ "Apple", "Orange", "Banana", "Tomato", "Milk", "Potato"];
    var axisX = ["10%", "20%", "30%", "40%", "50%", "60%", "70%", "80%", "90%", "100%"];
    var barsValue = [50, 61, 93, 76, 5, 13];

    // Data to charts
    var data = {
       "axisY": axisY,         // Data for axis Y labels
       "axisX": axisX,         // Data for axis X labels
       "bars": barsValue       // Data for bars value
    };

    // My options
    var options = {
        data: data,
        showValues: true,
        showHorizontalLines: true,
        animation: true,
        animationOffset: 0,
        labelsAboveBars: true
    };

    var options2 = {
        data: data,
        showValues: true,
        showHorizontalLines: true,
        animation: true,
        animationOffset: 0,
        animationRepeat: false,
        showArrows: false,
        labelsAboveBars: false
    };

    var chart = $('#chart-1').rumcaJS(options2);
    var chart2 = $('#chart-2').rumcaJS(options);

    chart2.sortByValue();

    console.log('%c Hello RumcaJS ' , 'font-size:24px; background: #000; color: #fff');
    //*************************************************************************
    //  Methods
    /**************************************************************************
    $myChart.horizontalChart(options);                          // Initialization horizontal chart.

    $myChart.resetAll();                                        // Remove all data.
    $myChart.resetBars();                                       // Remove all bars.
    $myChart.resetAxisY();                                      // Remove all data from axis Y.
    $myChart.resetAxisX();                                      // Remove all data from axis X.

    $myChart.removeItem(4);                                     // Remove single item. Parameter: int value (from the top, starting on 1).

    $myChart.appendAll(data);                                   // Insert all data. Parameter: object with data.
    $myChart.appendItem('new item', 33);                        // Insert an element to the end. Parameters: string value (for axis Y label), int value (for bar).
    $myChart.appendBars(barsValue);                             // Insert a bars to the end. Parameter: array with int value.
    $myChart.appendAxisY(axisY);                                // Insert an axis Y value to the end. Parameter: array with string value.
    $myChart.appendAxisX(axisX);                                // Insert an axis X value to the ending. Parameter: array with string value.

    $myChart.prependAll(data);                                  // Insert all data. Parameter: object with data.
    $myChart.prependItem('new item', 76);                       // Insert an element to the beginning. Parameters: string value (for axis Y label), int value (for bar).
    $myChart.prependBars(barsValue);                            // Insert a bars on the beginning. Parameter: array with int value.
    $myChart.prependAxisY(axisY);                               // Insert an axis Y value to the beginning. Parameter: array with string value.
    $myChart.prependAxisX(axisX);                               // Insert an axis X value to the beginning. Parameter: array with string value.

    $myChart.updateAll(data);                                   // Update chart with new data. Parameter: object with new data.
    $myChart.updateBars(barsValue);                             // Update a bars. Parameter: array with int value.
    $myChart.updateAxisY(axisY);                                // Update an axis Y. Parameter: array with string value.
    $myChart.updateAxisX(axisX);                                // Update an axis X. Parameter: array with string value.

    $myChart.sortByName(true);                                  // Sort by name. Parameter: boolean value (true - descending, false - ascending).
    $myChart.sortByValue(false);                                // Sort by value. Parameter: boolean value (true - descending, false - ascending).

    $myChart.selectMax();                                       // Select bar with maxiumum value.
    $myChart.selectMin();                                       // Select bar with minimum value.

    $myChart.runAnimation();                                    // Animation trigger.
    **************************************************************************/
});
