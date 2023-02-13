# FrameWork

FrameWork

### 시작 할때 Checklist

- package Mathematics 다운받기!
- 하면 편한 Package https://github.com/mob-sakai/ParticleEffectForUGUI.git  -  UI Particle
- 2D Sprite
- Cinemachine
- Project setting > Player > Publishing settings > Project key setting  path : Assets/Plugins/MondayOFF_Keystore.keystore / password : MondayOFF!@
- Rotation > Portrait

### 맨 처음 쓴다면! 

- URP를 사용하지 않는다면 Scene 카메라와 Light 에 Missing Script 빼주기
- Scene 마다 해당 BaseScene을 상속받는 Scene오브젝트를 넣는다
- 씬에 있는 @Scene 오브젝트에 GameScene script 실행됨 > UI GameScene 호출 > GameManager.GameInit()   (!!!물론 원하는때에 넣으면 됩니다 이건 그냥 예시용!!!)
- 
- Managers 를 호출하면 Managers 저절로 생성됨
- Managers 에서 타겟프레임 60으로 잡았고 멀티 터치 안되게 막아놓았음!
- Managers 는 DontDestroy 이고 나머지 매니저는 Managers에서 생산함
- Game Manager만 예외로 MonBehaviour를 사용하여 오브젝트로 생성합니다. (!!디버깅의 편리함!!)
- Managers 안에 Update에서 인터넷 연결을 확인함 삭제 해도됩니다!
- 새로운씬으로 갈때마다 Managers에서 Clear가 호출됨
- 
- DataManger는 Managers.Data 로 호출되며 EasySave3을 사용함
- 
- 풀링하는 객체는 Poolable 스크립트를 달아주어야 함
- 풀링매니저는 웬만하면 안써도 됨, 리소스 매니저의 Instance와 Destroy를 활용할 것
- 
- 모든 리소스는 리소스 매니저를 활용해서 처리할 것!
- 
- 씬 이동은 Scene 매니저를 활용함
- 씬 이동시에 SceneTrasition 애니메이션 실행 됨 (!!애니메이션은 바꿔서 활용할 것 있는것은 그냥 예시용!!)
- Define.Scene 에 씬들의 이름을 넣어주어야함
- 
- 사운드 관련내용은 Managers.Sound
- UI 관련 내용은 Managers.UI
- 버튼은 AddButtonEvent 함수를 사용하면 작았다 커지는 애니메이션을 사용함 ( Extension 에 AddButtonEvent 함수에 있습니다 )
- 해보고 모르겠으면 문의!!
- 
- Utils 폴더에는 여러 스크립트가 있는데
- Define 은 상수정의나 Enum정의때 사용
- Extension은 Extension기능을 위한 기능 추가 모음집
- Util 은 아무곳에나 활용가능한 Static 함수 집합
- 
- 
- 
- UI Particle 은 Canvas가 Screen Space - Overlay 인 경우 Particle을 활용하기 위해 파티클 상위 하이어라키에 붙여서 활용
- 
- Anti-Cheat Toolkit 은 메모리 탐지에 조금 더 안전하도록 하는 에셋  https://mentum.tistory.com/390- 
- 
- DOTween > Using DG.Tweeing 으로 사용
-  
- EasySave에 대해 정리 되있는 링크 https://mentum.tistory.com/156 
- 
- Editor Console Pro > 그냥 Unity Console보다 기능이 많음
- 
- Maintainer는 사용하는 레퍼런스를 알려주고 정리하는 에셋
- 
- Odin 관련 내용은 https://mentum.tistory.com/388 여기나 Tools > Odin Inspector > Attribute Example 을 활용하여 사용
- 
- Odin Validator 오류가 있는것을 찾아내는 에셋
- 
- 프로젝트 탭에서 폴더 Alt + 클릭 하면 폴더 색 바꿀수 있음 (Rainbow Folder2)
- 
- 씬에서 우클릭시 겹쳐있는 오브젝트 리스트가 나옴 (Selection Utility)
- 
- True Shadow 는 Shadow나 outline 대신 사용 가능


### Imported Assets

- 2D Sprite        Version 1.0.0 - July 27, 2022
- Cinemachine      Version 2.8.9 - August 31, 2022
- UniversalPR      Version 12.1.7 - July 27, 2022
- Addressables     Version 1.19.19 - March 04, 2022
- ProBuilder       Version 5.0.6 - July 08, 2022
- UI Particle      Version 4.1.7

- Anti-Cheat Toolkit Version 2021.6.3 - December 19, 2022
- DOTween          Version 1.0.335 - October 10, 2022
- Easy Save        Version 3.5.3 - December 12, 2022
- Editor Console Pro Version 3.971 - April 06, 2022
- Maintainer       Version 1.16.0 - September 19, 2022
- Odin             Version 3.1.9 - December 06, 2022
- Odin Validator   Version 3.1.9 - December 06, 2022
- Rainbow Folder2  Version 2.4.0 - May 07, 2022
- Selection Utility Version 1.2 - April 20, 2021
- True Shadow      Version 1.4.4 - December 14, 2022




### 업데이트 리스트 

