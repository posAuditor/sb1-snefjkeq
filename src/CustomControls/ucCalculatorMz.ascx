<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucCalculatorMz.ascx.cs" Inherits="CustomControls_ucCalculatorMz" %>

<style type="text/css">
    @import 'https://fonts.googleapis.com/css?family=Share+Tech+Mono';

    body {
        background: antiquewhite;
    }

    #header {
        text-align: center;
        font-family: 'Share Tech Mono', monospace;
    }

    #calc {
        text-align: center;
        /*width: 380px;*/
        display: block;
        border-radius: 8px;
        border: 1px solid;
        bordel-color: #abc6c2;
        padding: 8px;
        margin-top: 20px;
        margin-left: auto;
        margin-right: auto;
        background: #224662;
    }

    #display {
        background: #bcbcbc;
        padding: 8px;
        margin: 16px 12px 10px 16px;
        text-align: center;
        font-family: 'Share Tech Mono', monospace;
        border-radius: 8px;
    }

    #result p {
        font-size: 1.8em;
    }

    #result,
    #previous {
        text-align: right;
    }

    #keyboard {
        /*display: inline-block;*/
        text-align: center;
        margin-bottom: 8px;
        padding: 0px 15px 0px 0px;
    }

    .row {
        margin-top: 4px;
    }

    .last-row {
        float: left;
        margin-top: -11.5%;
    }

    .btn {
        width: 62px;
        margin: 2px;
    }

    .invisible {
        width: 0;
    }

    .btn-zero {
        width: 134px;
    }

    .btn-result {
        float: right;
        margin-left: 4px;
        height: 74px;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        var eq = "";
        var curNumber = "";
        var result = "";
        var entry = "";
        var reset = false;

        $(".btn").click(function () {
            entry = $(this).attr("value");

            if (entry === "ac") {
                entry = 0;
                eq = 0;
                result = 0;
                curNumber = 0;
                $('#result p').html(entry);
                $('#previous p').html(eq);
            }

            else if (entry === "ce") {
                if (eq.length > 1) {
                    eq = eq.slice(0, -1);
                    $('#previous p').html(eq);
                }
                else {
                    eq = 0;
                    $('#result p').html(0);
                }

                $('#previous p').html(eq);

                if (curNumber.length > 1) {
                    curNumber = curNumber.slice(0, -1);
                    $('#result p').html(curNumber);
                }
                else {
                    curNumber = 0;
                    $('#result p').html(0);
                }

            }

            else if (entry === "=") {
                result = eval(eq);
                $('#result p').html(result);
                eq += "=" + result;
                $('#previous p').html(eq);
                eq = result;
                entry = result;
                curNumber = result;
                reset = true;
            }

            else if (isNaN(entry)) {   //check if is not a number, and after that, prevents for multiple "." to enter the same number
                if (entry !== ".") {
                    reset = false;
                    if (curNumber === 0 || eq === 0) {
                        curNumber = 0;
                        eq = entry;
                    }
                    else {
                        curNumber = "";
                        eq += entry;
                    }
                    $('#previous p').html(eq);
                }
                else if (curNumber.indexOf(".") === -1) {
                    reset = false;
                    if (curNumber === 0 || eq === 0) {
                        curNumber = 0.;
                        eq = 0.;
                    }
                    else {
                        curNumber += entry;
                        eq += entry;
                    }
                    $('#result p').html(curNumber);
                    $('#previous p').html(eq);
                }
            }

            else {
                if (reset) {
                    eq = entry;
                    curNumber = entry;
                    reset = false;
                }
                else {
                    eq += entry;
                    curNumber += entry;
                }
                $('#previous p').html(eq);
                $('#result p').html(curNumber);
            }


            if (curNumber.length > 10 || eq.length > 26) {
                $("#result p").html("0");
                $("#previous p").html("Too many digits");
                curNumber = "";
                eq = "";
                result = "";
                reset = true;
            }

            if (result.indexOf(".") !== -1) {
                result = result.truncate()
            }

        });


    });
</script>

<div class="container">
    <div id="header">
        <h3>Calculator</h3>
    </div>
    <div id="calc" class="text-center">
        <div id="display">
            <div id="result">
                <p>0</p>
            </div>
            <div id="previous">
                <p>0</p>
            </div>
        </div>
        <div id="keyboard">
            <div class="row">
                <input type="button" class="btn btn-danger col-md-3" value="ce" />
                <input type="button" class="btn btn-danger col-md-2" value="ac" />
                <input type="button" class="btn btn-info col-md-2" value="9" />
                <input type="button" class="btn btn-info col-md-2" value="8" />
                <input type="button" class="btn btn-info col-md-2" value="7" />
            </div>
            <div class="row">
                <input type="button" class="btn btn-warning col-md-3" value="*" />
                <input type="button" class="btn btn-warning col-md-2" value="/" />
                <input type="button" class="btn btn-info col-md-2" value="6" />
                <input type="button" class="btn btn-info col-md-2" value="5" />
                <input type="button" class="btn btn-info col-md-2" value="4" />                
            </div>
            <div class="row">
                <input type="button" class="btn btn-success col-md-3" value="="/>
                <input type="button" class="btn btn-warning col-md-2" value="+" />
                <input type="button" class="btn btn-info col-md-2" value="3" />
                <input type="button" class="btn btn-info col-md-2" value="2" />
                <input type="button" class="btn btn-info col-md-2" value="1" />
            </div>
            <div class="row">                
                <!-- <input type="button" class="invisible" value=""/> -->
                <!-- <input type="button" class="invisible" value=""/> -->
                <input type="button" class="btn btn-warning col-md-3" value="-" />
                <input type="button" class="btn btn-warning col-md-2" value="." />
                <input type="button" class="btn btn-info btn-zero col-md-2" value="000" />
                <input type="button" class="btn btn-info btn-zero col-md-2" value="00" />
                <input type="button" class="btn btn-info btn-zero col-md-2" value="0" />
            </div>
        </div>
    </div>
</div>
