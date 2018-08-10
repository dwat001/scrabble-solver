﻿$(function () {
    var handleTab = function (event) {
        if (event.which == 9) { // Tab
            moveNextCell();
            event.preventDefault();
        }
    };
    var cellKeyPress = function (event) {
        var currentText = event.target;
        var row = parseInt(currentText.dataset.row);
        var column = parseInt(currentText.dataset.column);
        console.log("CellKeyPress Row: " + row + " Column:" + column);
        switch (event.which) {
            case 9: // Tab
                event.preventDefault();
                return;
            case 37: // left
                moveToCell(row, column - 1);
                event.preventDefault();
                return;

            case 38: // up
                moveToCell(row - 1, column);
                event.preventDefault();
                return;

            case 39: // right
                moveToCell(row, column + 1);
                event.preventDefault();
                return;

            case 40: // down
                moveToCell(row + 1, column);
                event.preventDefault();
                return;
        }
        if (event.which >= 65 && event.which <= 90) {
            var newChar = String.fromCharCode(event.which + 32);
            event.preventDefault();
            moveNextCell();
        }

    };

    var moveNextCell = function () {
        var currentText = document.activeElement;
        var row = parseInt(currentText.dataset.row);
        var column = parseInt(currentText.dataset.column);

        var direction = getWordOrintation();
        var nextRow = row + direction.row;
        var nextColumn = column + direction.column;

        moveToCell(nextRow, nextColumn);

    };

    var moveToCell = function (row, column) {
        var nextCellId = "Board_" + row + "__" + column + "__Cell_Letter";
        var nextCell = document.getElementById(nextCellId);
        console.log("MoveToCell("+row+","+column+") NextCellID="+nextCellId+" NextCell:" +nextCell+ ".");
        if (nextCell) {
            nextCell.focus();
        }
    };

    var getWordOrintation = function () {
        if (document.forms[0].direction.value === "tb") {
            return { row: 1, column: 0 };
        } else {
            return { row: 0, column: 1 };
        }
    };

    $('.double-letter').attr("placeholder", "2xL");
    $('.tripple-letter').attr("placeholder", "3xL");
    $('.double-word').attr("placeholder", "2xW");
    $('.tripple-word').attr("placeholder", "3xW");
    $("table.board input").keyup(cellKeyPress);
    $("table.board input").keydown(handleTab);
});