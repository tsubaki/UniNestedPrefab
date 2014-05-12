UniNestedPrefab
===============

プレハブのネスト化する奴

##使い方的な

*  **PrefabInPrefab**コンポーネントをアタッチし、Prefabに出力したいプレハブを登録しておくと、インスタンス時に自動的にプレハブを生成する。
*  PrefabInPrefabがアタッチされているプレハブを更新するには、**PrefabInPrefabController**の**プレハブ更新**で更新する。Applyすると子プレハブも一緒に登録されてしまうので駄目絶対。  
（プレハブ更新で更新すると、PrefabInPrefabで生成するオブジェクトを一旦退避してから登録する）

##楽に登録する

*  プレハブに**PrefabInPrefabController**を追加
*  上のプレハブの下にプレハブを追加
*  PrefabInPrefabControllerの**子プレハブを登録**を実行
*  **プレハブを更新**でプレハブを更新する

##プレハブの更新

*  **PrefabInPrefab**のPrefabを更新
*  **PrefabInPrefab**の**Reset Prefab**を実行

---

##課題

*  PrefabInPrefabコンポーネント登録するの面倒くさい。自動化したい。Prefab以下にPrefabを置いた場合、自動的に登録されるべき。
*  Applyを使うべき項目とUpdate Prefabを使う項目があって面倒くさい。PrefabInPrefabがある場合はApplyを上書きしたい
*  InspectorのプレハブのPreviewに映らない