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

#1. 트레일 

렌더러 알파(투명도) 미적용 문제
문제: Trail Renderer의 Gradient Alpha를 조절해도 화면에서 투명도가 적용되지 않고 진하게 출력됨.

원인 분석: 1. 사용 중인 URP Lit 셰이더의 기본 Surface Type이 Opaque로 설정되어 있었음.
2. 트레일의 Vertex Color 데이터를 셰이더에서 제대로 연산하지 못함.

해결 방법: 1. 트레일 전용 머터리얼을 분리 생성 후 Surface Type을 Transparent로 변경.
2. Generate Lighting Data 옵션을 활성화하여 트레일 메시의 노멀 데이터를 생성, Lit 머터리얼과의 조명 일관성 확보.
3. 필요에 따라 Particles/Lit 셰이더로 교체하여 정점 색상 데이터 동기화 완료.

**코드 아키텍처**
GameManager: 싱글톤 패턴을 활용한 게임 상태 관리 및 점수 시스템 제어.

Object Pooling: (만약 적용하셨다면) 빈번한 복제/파괴로 인한 GC 부하를 줄이기 위한 풀링 기법 적용.

**데모 (Youtube)**
https://youtu.be/XfxsCGON0nA
