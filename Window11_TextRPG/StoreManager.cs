﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Window_11_TEXTRPG;

namespace Window11_TextRPG
{
    internal class StoreManager : IScene
    {
        // Singleton
        private StoreManager() { }
        private static StoreManager? instance;
        public static StoreManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new StoreManager();
                return instance;
            }
        }
        // 인벤토리에서 가져온 아이템 리스트
        List<MountableItem> items = InventoryManager.Instance.mountableItems;

        // 임시 변수들
        Player player = new Player();    

        // GameManager에서 접근하는 곳
        public void Enter()
        {
            // StoreScene 진입
            StoreScene();
        }

        private void StoreScene()
        {
            // 다음 Scene 지정
            Action nextScene = StoreScene;
            int result = -1;

            do
            {
                // StoreScene 출력
                DisplayManager.StoreScene(player, items);
                result = UtilManager.PlayerInput(0, 2);

                switch (result)
                {
                    case 0: // 로비
                        Console.WriteLine("debug로비로 간다");
                        Thread.Sleep(1000);
                        nextScene = StoreScene;
                        break;

                    case 1: // 구매 페이지
                        nextScene = BuyItemScene;
                        break;

                    case 2: // 판매 페이지
                        nextScene = SellItemScene;
                        break;
                }
            }
            while (result < 0 || result > 2);

            nextScene();
        }

        private void BuyItemScene()
        {
            // 다음 Scene 지정
            Action nextScene = BuyItemScene;

            int result = -1;
            int inputCount = items.Count() + 1;
            do
            {
                // DiplayManager 접근
                DisplayManager.StoreBuyScene(player, items);
                
                result = UtilManager.PlayerInput(0, inputCount);

                // 나가기 버튼
                if (0 == result)
                {
                    nextScene = StoreScene;
                }
                // 아이템 구매 접근
                else
                {
                    BuyItem(result);
                }
                
            }
            while (result < 0 || result > inputCount);

            nextScene();
        }

        private void SellItemScene()
        {
            // 다음 Scene 지정
            Action nextScene = BuyItemScene;

            int result = -1;
            int inputCount = items.Count() + 1;
            do
            {
                // DiplayManager 접근
                DisplayManager.StoreBuyScene(player, items);

                result = UtilManager.PlayerInput(0, inputCount);

                // 나가기 버튼
                if (0 == result)
                {
                    nextScene = StoreScene;
                }
                // 아이템 판매 접근
                else
                {
                    SellItem(result);
                }

            }
            while (result < 0 || result > inputCount);

            nextScene();
        }

        private void BuyItem(int idx)
        {
            // 구매 불가
            if (items[idx - 1].Own)
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                Thread.Sleep(1000);
            }
            // 구매 가능
            else
            {
                // Gold 충분
                if (items[idx - 1].Price <= player.gold)
                {
                    items[idx - 1].Own = true;
                    player.gold -= items[idx - 1].Price;
                    Console.WriteLine("구매를 완료했습니다.");
                    Thread.Sleep(1000);
                }
                // Gold 부족
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                    Thread.Sleep(1000);
                }
            }
        }

        private void SellItem(int idx)
        {
            MountableItem item = items[idx - 1];
            // 판매 불가
            if (!item.Own)
            {
                Console.WriteLine("소유하지 않은 아이템입니다.");
                Thread.Sleep(1000);
            }
            // 판매 가능
            else
            {
                // 장착 중이라면 장착 해제
                if (item.Equip)
                {
                    // 아이템 성능 만큼 캐릭터 성능 하향
                    //equipmentScene.MountItem(item, false);
                }
                item.Own = false;
                player.gold += (int)((float)item.Price * 0.85f);
                Console.WriteLine("판매를 완료했습니다.");
                Thread.Sleep(1000);
            }
        }

    }
}

