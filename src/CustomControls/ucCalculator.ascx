<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucCalculator.ascx.cs" Inherits="CustomControls_ucCalculator" %>

<style>
   
    .box {
        /*background-color: #3d4543;*/
        height: 300px;
        width: 250px;
        /*border-radius: 10px;
        position: relative;
        top: 80px;
        left: 40%;*/
    }

    .display {
        background-color: #222;
        width: 220px;
        position: relative;
        /*left: 15px;
        top: 20px;*/
        height: 40px;
    }

        .display input {
            position: relative;
            left: 2px;
            top: 2px;
            height: 43px;
            color: black;
            background-color: white;
            font-size: 21px;
            text-align: right;
        }

    .keys {
        position: relative;
        top: 15px;
    }

    .button {
        width: 40px;
        height: 40px;
        border: none;
        border-radius: 8px;
        margin-left: 17px;
        cursor: pointer;
        border-top: 2px solid transparent;
        /*color:black!important;*/
    }

        .button.gray {
            color: white;
            background-color: #6f6f6f;
            border-bottom: black 2px solid;
            border-top: 2px #6f6f6f solid;
        }

        .button.pink {
            color: white;
            background-color: #662d91;
            border-bottom: black 2px solid;
        }

        .button.black {
            color: white;
            background-color: #303030;
            border-bottom: black 2px solid;
            border-top: 2px #303030 solid;
        }

        .button.orange {
            color: black;
            background-color: FF9933;
            border-bottom: black 2px solid;
            border-top: 2px FF9933 solid;
        }

    .gray:active {
        border-top: black 2px solid;
        border-bottom: 2px #6f6f6f solid;
    }

    .pink:active {
        border-top: black 2px solid;
        border-bottom: #ff4561 2px solid;
    }

    .black:active {
        border-top: black 2px solid;
        border-bottom: #303030 2px solid;
    }

    .orange:active {
        border-top: black 2px solid;
        border-bottom: FF9933 2px solid;
    }

    p {
        line-height: 10px;
    }
</style>


<script>
    function c(val) {
        document.getElementById("d").value = val;
    }
    function v(val) {
        document.getElementById("d").value += val;
    }
    function e() {
        try {
            c(eval(document.getElementById("d").value))
        }
        catch (e) {
            c('Error')
        }
    }
</script>



<div class="box">
    <div class="display">
        <input type="text" readonly="" size="18" id="d">
    </div>
    <div class="keys">
        <p>
            <input type="button" class="button gray" value="mrc" onclick="c(&quot;Created....................&quot;)"><input type="button" class="button gray" value="m-" onclick="    c(&quot;...............by............&quot;)"><input type="button" class="button gray" value="m+" onclick="    c(&quot;.....................Anoop&quot;)"><input type="button" class="button pink" value="/" onclick="    v(&quot;/&quot;)">
        </p>
        <p>
            <input type="button" class="button black" value="7" onclick="v(&quot;7&quot;)"><input type="button" class="button black" value="8" onclick="    v(&quot;8&quot;)"><input type="button" class="button black" value="9" onclick="    v(&quot;9&quot;)"><input type="button" class="button pink" value="*" onclick="    v(&quot;*&quot;)">
        </p>
        <p>
            <input type="button" class="button black" value="4" onclick="v(&quot;4&quot;)"><input type="button" class="button black" value="5" onclick="    v(&quot;5&quot;)"><input type="button" class="button black" value="6" onclick="    v(&quot;6&quot;)"><input type="button" class="button pink" value="-" onclick="    v(&quot;-&quot;)">
        </p>
        <p>
            <input type="button" class="button black" value="1" onclick="v(&quot;1&quot;)"><input type="button" class="button black" value="2" onclick="    v(&quot;2&quot;)"><input type="button" class="button black" value="3" onclick="    v(&quot;3&quot;)"><input type="button" class="button pink" value="+" onclick="    v(&quot;+&quot;)">
        </p>
        <p>
            <input type="button" class="button black" value="0" onclick="v(&quot;0&quot;)"><input type="button" class="button black" value="." onclick="    v(&quot;.&quot;)"><input type="button" class="button black" value="C" onclick="    c(&quot;&quot;)"><input type="button" class="button orange" value="=" onclick="    e()">
        </p>
    </div>
</div>
