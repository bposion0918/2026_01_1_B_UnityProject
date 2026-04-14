using JetBrains.Rider.Unity.Editor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float movespeed = 5.0f;
    public float jumpforce = 7.0f;
    public float turnSpeed = 10.0f;             //회전 속도

    [Header("점프 개선 설정")]
    public float falllMultiplier = 2.5f;        //하강 중력 배율
    public float lowJumpMultiplier = 2.0f;      //짧은 점프 배율

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;            //지면 관성 시간
    public float coyoteTimeCounter;             //관성 타이머
    public bool realGrounded = true;            //실제 지면 상태

    [Header("글라이더 설정")]
    public GameObject gliderObject;             //글라이더 오브젝트
    public float gliderFallSpeed = 1.0f;        //글라이더 하강 속도
    public float gliderMoveSpeed = 7.0f;        //글라이더 이동 속도
    public float gliderMaxTime = 5.0f;          //글라이더 최대 시간
    public float gliderTimeLeft;                //글라이더 남은 시간
    public bool isGliding = false;              //글라이딩 중인지 여부

    public Rigidbody rb;                        //플레이어 강체 선언

    public bool isGrounded = true;

    public int coinCount = 0;

    void Start()
    {
        coyoteTimeCounter = 0;

        if (gliderObject != null)                  //글라이더 오브젝트 초기화
        {
            gliderObject.SetActive(false);        //게임 시작 시 글라이더 비활성화
        }

        gliderTimeLeft = gliderMaxTime;           //글라이더 시간 초기화
    }

    // Update is called once per frame
    void Update()
    {
        //지면 감지
        UpdateGroundedState();

        //움직임 입력
        float moveHorizontal = Input.GetAxis("Horizontal");   //수평 이동
        float moveVertical = Input.GetAxis("Vertical");       //수직 이동

        //이동 방향 벡터
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);         //이동 방향 감지

        if (movement.magnitude > 0.1f)        //입력이 있을 때만 회전
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);          //이동 방향을 바라보도록 부드럽게 회전
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        //G키로 글라이더 제어 (누르는 동안 활성화)
        if (Input.GetKey(KeyCode.G) && !isGrounded && gliderTimeLeft > 0) //G키를 누르며너 땅에 있지 않고 글라이더 남은시간이 있을 때 (3가지 조건)
        {
            if (!isGliding)                      //글라인더 활성화
            {
                EnableGlider();
            }
            //글라이더 시간 감소
            gliderTimeLeft -= Time.deltaTime;

            //글라이더 시간이 다 되면 비활성화
            if (gliderTimeLeft <= 0)
            {
                DisableGlider();
            }
        }
        else if (isGliding)
        {
            DisableGlider();                       //G키를 떼면 글라이더 비활성화
        }

        if (isGliding)
        {
            ApplyGliderMovement(moveHorizontal, moveVertical);        //글라이더 이동 적용
        }

        else
        {
            //속도 값으로 직접 이동
            rb.linearVelocity = new Vector3(moveHorizontal * movespeed, rb.linearVelocity.y, moveVertical * movespeed);

            //착시 점프 높이 구현
            if (rb.linearVelocity.y < 0)       //하강 중일 때
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (falllMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))                      //상승 중 점프 버튼을 떄면 낮게 점프
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

            //점프 입력
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
                isGrounded = false;
                realGrounded = false;
                coyoteTimeCounter = 0;
            }

            if (isGrounded)
            {
                if (isGliding)
                {
                    DisableGlider();                   //지면에 닿으면 글라이더 비활성화
                }

                gliderTimeLeft = gliderMaxTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)            //충돌 처리 함수
    {

        if (collision.gameObject.tag == "Ground")                 //충돌이 알어난 물체의 Tag가 Ground인 경우
        {
            realGrounded = true;                                    //땅과 충돌하면 True로 변경한다
        }
    }

    private void OnCollisionStay(Collision collision)             //지면과의 충돌이 유지되는지 확인
    {

        if (collision.gameObject.tag == "Ground")                 //충돌이 알어난 물체의 Tag가 Ground인 경우
        {
            realGrounded = true;                                    //땅과 충돌하면 True로 변경한다
        }
    }

    private void OnCollisionExit(Collision collision)             //지면에서 떨어졌는지 확인
    {

        if (collision.gameObject.tag == "Ground")
        {
            realGrounded = false;                                   //지면에서 떨어졌기 때문에 False
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
        }
    }

    //지면 상태 업데이트 함수
    void UpdateGroundedState()
    {
        if (realGrounded)
        {
            coyoteTimeCounter = coyoteTime;       //지면에 있을 때 카우보이 타임 초기화
            isGrounded = true;

        }
        else
        {
            if (coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;       //카우보이 타임 감소
                isGrounded = true;                       //카우보이 타임 동안 점프 허용

            }
            else
            {
                isGrounded = false;                      //타임이 끝나면 False
            }
        }
    }

    //글라이더 활성화 함수

    void EnableGlider()
    {
        isGliding = true;

        //글라이더 오브젝트 표시
        if (gliderObject != null)
        {
            gliderObject.SetActive(true);
        }

        //하강 속도 초기화
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, -gliderFallSpeed, rb.linearVelocity.z);
    }

    //글라이더 비활성화 함수
    void DisableGlider()
    {
        isGliding = false;

        //글라이더 오브젝트 숨기기
        if (gliderObject != null)
        {
            gliderObject.SetActive(false);
        }

        //즉시 낙하하도록 중력 적용
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
    }

    //글라이더 이동 적용
    void ApplyGliderMovement(float horizontal, float vertical)
    {
        //글라이더 효과 : 천천히 떨어지고 수평 방향으로 더 빠르게 이동
        Vector3 gliderDirection = new Vector3(
            horizontal * gliderMoveSpeed,
            -gliderFallSpeed,
            vertical * gliderMoveSpeed
         );

        rb.linearVelocity = gliderDirection;       //글라이더 이동 적용
    }
}
