using UnityEngine;

public class MyCharacrer : MonoBehaviour
{
    public int Health = 100;                //체력을 선언 한다. (변수 정수 표현)
    public float Timer = 1.0f;              //타이머를 설정 한다. (변수 실수 표현)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = Health + 100;               //첫 시작 할 때 100의 체력을 추가한다.
    }

    // Update is called once per frame
    void Update()
    {
        Timer = Timer - Time.deltaTime;     //시간을 매 프레임마다 감소시킨다. (deltaTime은 프레임간의 시간 간격을 의미한다)

        if (Timer <= 0)                     //만약 Timer의 수치가 0이하로 내려갈 경우
        {
            Timer = 1.0f;                   //다시 1초로 변경 시켜 준다.
            Health = Health - 20;           //1초마다 체력이 20 줄어 든다.
        }

        if (Input.GetKeyUp(KeyCode.Space))  //스페이스 키를 눌렀을 때
        {
            Health = Health + 2;            //체력 포인트를 2 올려준다.
        }

        if (Health <= 0)                    //체력이 0 이하가 된 경우
        {
            Destroy(this.gameObject);       //이 오브젝트를 없엔다.
        }
    }
}
            