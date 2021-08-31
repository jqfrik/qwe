function loginTry() {
    let loginElem = document.querySelector("#auth_login");
    let passElem = document.querySelector("#auth_login_password");
    let enterBtn = document.querySelector("#auth > div.flex-row.flex-end.login-pass > div.flex-col.flex-row--masked > div > div:nth-child(2) > div > div:nth-child(1) > div > div > button");
    simulate(loginElem, "ilya13072000");
    simulate(passElem, "wspui5DKUY5nFqy");
    enterBtn.click();
}
function simulate(elem, newValue) {
    let oldValue = elem.value;

    let event = new Event('input', { bubbles: true });
    event.simulated = true;

    elem.value = newValue;

    let tracker = elem._valueTracker;
    if (tracker) {
        tracker.setValue(oldValue);
    }

    elem.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));
    elem.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));
    elem.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));
    elem.dispatchEvent(new Event('input', { bubbles: true }));
    elem.dispatchEvent(new Event('change', { bubbles: true }));
}
function getBalance() {

}
async function findStake() {
    //вся линия
    let allBetLinesLoaded = await asyncRepeater(() => {
        let allBetLines = document.getElementsByClassName("bg coupon-row")[0];
        if (allBetLines == null) {
            return false;
        }
        return true;
    }, 5, 500);
    if (!allBetLinesLoaded) {
        console.log("линия не прогрузилась");
        //нужно сделать уведомление,что линия не прогрузилась
    }
    //все выборы(вставка для основных ставок,фор,тоталов,голов,таймы)
    let tabs = document.getElementsByClassName("table-shortcuts-menu")[0];
    //блоки ставок(для основных,тоталы,форы и другие)
    let allBlocks = document.getElementsByClassName("blocks-area")[0];

    let stake = null;
    //Есть форы,тоталы,голы,таймы,сеты,геймы,карты(в киберспорте победа на первой карте например)
    //статистика(кто совершит больше убийств,тотал убийств)

    let nameOfEvents = document.getElementsByClassName("coupon-row-item coupone-labels")[0];

    let stakesOnBk = {
        "п1": "1",
        "п2": "2",
        "x": "x",
        "1x": "1x",
        "12": "12",
        "x2": "x2",
        "ф1": "Фора1",
        "ф2": "Фора2",
        "тм": "Меньше",
        "тб": "Больше"
    };
    
    //worker.betValue
    let betValue = worker.betValue.toLowerCase();
    console.log("значение BetValue");
    console.log(betValue);
    let nameStakeOnBk = null;
    console.log("начало цикла");
    for (var key in stakesOnBk) {
        console.log("ключ " + key);
        console.log("betValue " + betValue);
        if (betValue.indexOf(key) != -1) {
            nameStakeOnBk = stakesOnBk[key];
            break;
        }
    }
    console.log("навзвание ставки на БК");
    console.log(nameStakeOnBk);
    let indexForStake = null;
    for (let i = 2; i < nameOfEvents.children[0].children[0].children.length; i++) {
        if (nameOfEvents.children[0].children[0].children[i].textContent.indexOf(nameStakeOnBk) != -1) {
            indexForStake = i;
        }
    }
    console.log("indexForStake");
    console.log(indexForStake);
    let trOnLine = document.getElementsByClassName("sub-row")[0];
    let finalStake = null;
    if (indexForStake != null) {
        finalStake = trOnLine.children[index + 1];
    }
    console.log("BetValue");
    console.log(betValue);
    /*
    if (betValue.toLowerCase().indexOf("п1") != -1 ) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[3];
    } else if (betValue.toLowerCase().indexOf("x") != -1 && betValue.length < 2) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[4];
    } else if (betValue.toLowerCase().indexOf("п2") != -1) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[5];
    } else if (betValue.toLowerCase().indexOf("1x") != -1 && betValue.length < 3) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[6];
    } else if (betValue.toLowerCase().indexOf("12") != -1 && betValue.length < 3) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[7];
    } else if (betValue.toLowerCase().indexOf("x2") != -1 && betValue.length < 3) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[8];
    } else if (betValue.toLowerCase().indexOf("ф1") != -1) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[9];
    } else if (betValue.toLowerCase().indexOf("ф2") != -1) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[10];
    } else if (betValue.toLowerCase().indexOf("тм") != -1) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[11];
    } else if (betValue.toLowerCase().indexOf("тб") != -1) {
        stake = document.getElementsByClassName("coupon-row-item")[1].children[0].children[0].children[12];
    }
    */
    console.log("конец поиска ставки");
    return finalStake;
    //return stake;
}
function inFirstRowStake(betValue) {
    return betValue.indexOf("1") != -1 && betValue.length < 2 || betValue.indexOf("X") != -1 && betValue.length < 2 || betValue.indexOf("2") != -1 && betValue.length < 2 || betValue.indexOf("1X") != -1 && betValue.length < 3 || betValue.indexOf("12") != -1 && betValue.length < 3 || betValue.indexOf("X2") != -1 && betValue.length < 3;
}
async function setStakeJs() {
    //await sleep(10000);
    console.log("начало ставки");

    //let stakeWinOnFirstTeam = document.querySelector("#category228686 > div > div > div.bg.coupon-row > table > tbody > tr > td.price.height-column-with-price.first-in-main-row.coupone-width-1");
    let stake = await findStake();
    console.log("Ставка " + !!stake);
    if (stake) {
        stake.click();
    } else {
        console.log("ставка не нашлась");
        return;
    }
    setTimeout(() => {
        let betSlipInput = document.querySelector('input[name^="stake"]');
        console.log("Инпут для ввода суммы " + !!betSlipInput);
        simulate(betSlipInput, 20);
        setTimeout(() => {
            let btnStake = document.querySelector("#betslip_placebet_btn_id");
            let blockChangeCondion = document.getElementById("betslip_apply_choices_block");
            let acceptCondition = document.querySelector("#betslip_apply_choices");
            if (blockChangeCondion.textContent.indexOf("Изменились условия") != -1) {
                acceptCondition.click();
            }
            //do {
                //await sleep(1000);
            //} while (btnStake.classList.contains("btn-place-bet-disabled"))
            btnStake.click();
            console.log("Кнопка для заключения пари " + !!btnStake);
            btnStake.click();

            /*await asyncRepeater(() => {
                let resultDialog = document.getElementById("result-dialog");
                if (resultDialog !== null) {

                }
            }, 5, 1000);*/
        }, 3000);
        console.log("конец ставки");
    }, 5000);
}
function getMaxStakeSum() {
    return document.querySelector("#max-stake-10995078\\,Match_Result\\.1").textContent.replace("\n", "").replace("\n", "");
}
function getMinStakeSum() {
    return document.querySelector("#min-stake-10995078\\,Match_Result\\.1").textContent;
}
async function asyncRepeater(callback,tryCount,msec) {
    let tryCounter = 0;
    while (tryCounter < tryCount) {
        let resultCallback = callback();
        if (resultCallback == true) {
            return true;
        }
        await sleep(msec);
    }
    return false;
}
async function sleep(msec) {
    return new Promise(resolve => setTimeout(resolve, msec));
}


worker.setLoginTry(loginTry);
worker.setStakeSetter(setStakeJs);