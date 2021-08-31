let isLoginSuccess = false;

let warningForLogin = document.querySelector("#auth > div:nth-child(1) > div.better-inputs__error-e03305");
let warningForPassword = document.querySelector("#auth > div.better-inputs__password-1460a4.auth-form__password-e95182 > div.better-inputs__error-e03305");

function loginTry() {
    let button = document.querySelector('#header-sign-in');
    button.click();

    let event = new Event('input', { bubbles: true });
    event.simulated = true;
    let eventTwo = new Event('input', { bubbles: true });
    eventTwo.simulated = true;

    let login = document.querySelector('#auth > div:nth-child(1) > input');
    let lastValueLogin = login.value;
    login.value = worker.login;
    let tracker = login._valueTracker;
    if (tracker) {
        tracker.setValue(lastValueLogin);
    }
    login.dispatchEvent(event);

    let pass = document.querySelector('#auth > div.better-inputs__password-1460a4.auth-form__password-e95182 > input');
    let lastValuePass = pass.value;
    pass.value = worker.password;
    let trackerPass = pass._valueTracker;
    if (trackerPass) {
        trackerPass.setValue(lastValuePass);
    }
    pass.dispatchEvent(eventTwo);
    let btnEnter = document.querySelector('#auth > button');
    btnEnter.removeAttribute('disabled');
    setTimeout(() => {
        btnEnter.click();
        worker.addMessageInLogger("Логин выполнен");
    }, 1000)
}

function checkLogin() {
    console.log("checklogin");
    if (isLoginSuccess) {
        clearInterval(interval);
    }
    if (warningForLogin == null && warningForPassword == null) {
        isLoginSuccess = true;
    } else {
    }
}

function getBalance() {
    const balanceText = document.querySelector('span[class="auth-panel__wallets-amount-9326f2"]').textContent;
    console.log(balanceText);
    console.log("баланс");
    return balanceText;
}

function setStakeJs() {
    //Блок который содержит табы(все пари,основные пари,тотал,фора,точный счёт,другие) и все ставки
    let elemForAllStakes = document.querySelector("#content > div > div.event-2b3c6d > div.event__wrapper-events-406718");

    //может содержать  строку "Приём пари временно приостановлен" нужно проверять чтобы бот ожидал пока ставки появятся либо пропускал
    //и открывал новую вилку
    //document.querySelector("#content > div > div.event-2b3c6d > div.event__wrapper-events-406718").innerText

    //Выбираем блок где содержатся ставки для основного времени

    let blockForStake = null;
    if (elemForAllStakes != null) {
        for (let i = 0; i < elemForAllStakes.children.length; i++) {
            if (elemForAllStakes.children[i].innerText.toLowerCase().includes("основное время")) {
                blockForStake = elemForAllStakes.children[i];
                break;
            }
        }
    }
    //worker.addMessageInLogger("blockForStake 111 строка");
    //Нужно найти ставку Победитель 
    let stakeWinner = document.querySelector("#content > div > div.event-2b3c6d > div.event__wrapper-events-406718 > div.part__markets-86eb26 > div:nth-child(1) > div > div:nth-child(1)");
    /*for (let i = 0; i < blockForStake.children.length; i++) {
        for (let j = 0; j < blockForStake.children[j].length; j++) {
            if (blockForStake.children[j].children[0].textContent == "Победитель") {
                stakeWinner = blockForStake.children[j];
                break;
            }
        }
    }*/
    //worker.AddMessageInLogger("StakeWinner 122 строка");
    //Выбираем ставку победитель первая команда/игрок
    console.dir(stakeWinner);
    stakeWinner.click();
    //Код ищет внутри победителя первую ставку
    //stakeWinner.children[1].children[0].click();

    //Инпут купона для ввода суммы ставки
    let betSlipInput = document.querySelector('input[class^="betslip__input"]');

    //вызов события на купоне
    let oldValue = betSlipInput.value;
    betSlipInput.value = 10;
    let event = new Event('input', { bubbles: true });
    event.simulated = true;
    if (betSlipInput._valueTracker) {
        betSlipInput._valueTracker.setValue(oldValue);
    }
    betSlipInput.dispatchEvent(event);

    //Нажимаем на кнопку заключить пари
    let betSlipButton = document.querySelector("#betslip__accept-and-submit");
    betSlipButton.click();
    //worker.AddMessageInLogger("Нажатие кнопки купона 141 строка");
    console.log("setStake закончил выполнение");
}


worker.SetStakeSetter(setStakeJs);
worker.setGetBalance(getBalance);
worker.setLoginTry(loginTry);