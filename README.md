# Unity3D RPG Portfolio
**개발기간** : 4주(2021.03.12 ~ 2021.04.12)   
**개발환경** : Unity 2019.3.0f6, SourceTree, Visual Studio 2019   
**타겟 플랫폼** : Android   
**개발인원** : 혼자   
**요약** : 유니티 엔진을 이용한 3D 던전탐색 RPG. 몬스터를 잡고 소환되는 보스를 처치하는 간단한 게임.    
   
## 기능
+ 몬스터 스포너
+ 스킬 
  + 스킬 교체
  + 체인 라이트닝
+ 보스
***
### 몬스터 소환
몬스터를 처치하면 좌측 상단의 Chaos수치가 상승하고, 50%가 채워지면 보스가 소환되게 구현했습니다.   
### 스킬
스킬은 총 3개로 구성했으며, 체인 라이트닝, 휠 윈드, 힐링 포션입니다.   
스킬은 Scriptable Object로 정의하고, 쿨타임과 데미지등의 변수를 저장했습니다.
***
#### 스킬 교체   
스킬의 쿨타임은 SkillManager클래스에서 전부 관리하고, 게임 내 UI에서 스킬을 퀵슬롯에 설정 가능하게 구현했습니다.   
스킬을 교체해도, 쿨타임은 그대로 유지되며 교체됩니다.   
+ 스킬 슬롯에 스킬을 등록할 수 있는 UI. 드래그 방식으로 변경
![SkillUI](https://user-images.githubusercontent.com/54834146/114380435-0afac000-9bc5-11eb-867f-ac95c45dffa8.PNG)   

+ 스킬 사용후, 같은 스킬들은 동시에 쿨타임이 돌게되는 모습   

![sameCooltime](https://user-images.githubusercontent.com/54834146/114380182-bb1bf900-9bc4-11eb-986c-5759eccda7c8.PNG)   
#### 체인 라이트닝
체인 라이트닝은 전기로 적을 공격하고, 첫 목표에서 가장 가까운 다른 적을 추적해 연달아 공격하는 스킬입니다.   

![ChainLightning](https://user-images.githubusercontent.com/54834146/114380753-63ca5880-9bc5-11eb-96fa-df38af3468d9.PNG)   
***
### 보스
보스는 따로 HP를 UI로 표시하고, 체력 수치에 따라 다른 패턴을 실행하도록 구현했습니다.   
+ 소환 패턴   
주변에 두마리의 스켈레톤을 소환하는 패턴입니다.   
![Boss_Summon](https://user-images.githubusercontent.com/54834146/114381023-a3914000-9bc5-11eb-8d2c-c1362e476229.PNG)   
+ 내려찍기 패턴   
플레이어 위치에 일정 시간 후 내려찍기 공격을 하는 패턴입니다.   
![Boss_Smash](https://user-images.githubusercontent.com/54834146/114381131-bd328780-9bc5-11eb-9d83-dac561b6fc2f.PNG)   

***
**후기**   
완성은 했지만, 코딩에도 아쉬움이 남고, 깔끔하게 완성한 것 같지 않아서 아쉬웠습니다.   
다시 유니티 관련으로 공부를 하고 취업을 위해 코딩테스트를 공부해야겠다고 느꼈습니다.
   
   
**영상 링크** : 
