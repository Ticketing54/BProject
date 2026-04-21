## ChangeAndDrop
<img width="548" height="391" alt="Animation" src="https://github.com/user-attachments/assets/283b9c9a-240d-4cff-8bca-b3aba19f66c0" />

**한 줄 소개**: 떨어지는 공을 원하는 박스에 떨어뜨려 많은 점수를 내는 게임

**개발 기간**: 2026.01 ~ 현재

**플랫폼**: PC (Mobile 확장 고려)

**사용 기술**: Unity 2022.3+, URP, C#

**핵심 목표**: 

1. 떨어지는 공들이 바글바글한 느낌을 살릴것,

2. 물리 충돌 및 효율적인 공의 복제 및 시각적 피드백 을 잘 보이도록 할 것 

3. 원본 게임과 최대한 유사하게 구현할것

**핵심 구현 사항**

이 프로젝트에서 공들여 구현한 기술적인 포인트들입니다.

A. 게임 상태 관리 시스템
FSM 기반 GameManager를 구현하여 게임 흐름을 상태 단위로 관리하고,
이벤트 기반 구조를 통해 각 시스템 간 결합도를 낮춘 구조 설계

B. 실시간 객체 복제 시스템
물리 기반의 볼 드롭 및 조건부 복제 로직 구현.
ObjectPool 을 이용한 공 관리 

C. 비주얼 피드백
Trail System: 이동 궤적을 시각화하여 속도감 부여.
stencil: 충돌게이트가 무한히 회전하는것처럼 표현

D. 맵 제작 간편화
모든 장애물 오브젝트를 인터페이스 기반 장애물 데이터화로 맵 재사용 구조 구현

E. 카메라 연출/ 제어
Cinemachine의 Virtual Camera와 Damping을 활용하여 타겟 추적 시 흔들림 없는 부드러운 카메라 이동을 구현


**트러블 슈팅**
개발 중 마주한 기술적 문제와 해결 과정입니다.
https://www.notion.so/TrounbleShooting-3490b606d4f480b192b2f726f24cf19d?source=copy_link

**코드 아키텍처**
https://www.notion.so/Architecture-3490b606d4f48079a86dda7a4223fe8e?source=copy_link

**데모 (Youtube)**
https://youtu.be/XfxsCGON0nA
