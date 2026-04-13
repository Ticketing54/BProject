🎮 프로젝트명 (예: Ball Drop & Replicate)
한 줄 소개: 공의 복제와 물리 연산을 활용한 캐주얼 유니티 게임 (현재 개발 진행률 70%)

🚀 1. 프로젝트 개요
개발 기간: 2026.03 ~ 현재

플랫폼: PC (Mobile 확장 고려)

사용 기술: Unity 2022.3+, URP, C#

핵심 목표: 효율적인 객체 복제 로직 구현 및 시각적 피드백(Trail, Particle) 최적화

🛠 2. 핵심 구현 사항
이 프로젝트에서 공들여 구현한 기술적인 포인트들입니다.

A. 실시간 객체 복제 시스템
물리 기반의 볼 드롭 및 조건부 복제 로직 구현.

ScriptableObject를 활용하여 다양한 볼의 속성(무게, 색상, 복제 확률 등)을 데이터화하여 관리.

B. URP 기반 비주얼 피드백
Trail System: 이동 궤적을 시각화하여 속도감 부여.

Dynamic Material: 실시간으로 변화하는 볼의 색상에 맞춰 트레일 색상 및 강도(Intensity) 동기화.

🔍 3. 트러블 슈팅 (Troubleshooting)
개발 중 마주한 기술적 문제와 해결 과정입니다.

#1. 트레일 렌더러 알파(투명도) 미적용 문제
문제: Trail Renderer의 Gradient Alpha를 조절해도 화면에서 투명도가 적용되지 않고 진하게 출력됨.

원인 분석: 1. 사용 중인 URP Lit 셰이더의 기본 Surface Type이 Opaque로 설정되어 있었음.
2. 트레일의 Vertex Color 데이터를 셰이더에서 제대로 연산하지 못함.

해결 방법: 1. 트레일 전용 머터리얼을 분리 생성 후 Surface Type을 Transparent로 변경.
2. Generate Lighting Data 옵션을 활성화하여 트레일 메시의 노멀 데이터를 생성, Lit 머터리얼과의 조명 일관성 확보.
3. 필요에 따라 Particles/Lit 셰이더로 교체하여 정점 색상 데이터 동기화 완료.

📂 4. 코드 아키텍처
GameManager: 싱글톤 패턴을 활용한 게임 상태 관리 및 점수 시스템 제어.

Object Pooling: (만약 적용하셨다면) 빈번한 복제/파괴로 인한 GC 부하를 줄이기 위한 풀링 기법 적용.

🎥 5. 데모 (GIF/Youtube)
(여기에 게임 플레이 GIF나 유튜브 링크를 넣으세요)
