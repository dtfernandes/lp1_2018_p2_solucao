// License: GPLv3
// Author: Nuno Fachada
using System;
using System.Collections.Generic;

namespace ZombiesVsHumans
{
    public class Game
    {

        private Options options;
        private IUserInterface ui;
        private Agent[,] world;
        private Agent[] agents;
        private Random rand;

        public Game(Options options, IUserInterface ui)
        {
            this.options = options;
            this.ui = ui;
            world = new Agent[options.XDim, options.YDim];
            agents = new Agent[options.Zombies + options.Humans];
            rand = new Random();

            for (uint i = 0; i < options.Zombies; i++)
            {

                if (i < options.PlayerZombies)
                {
                    // Create player-controlled zombie
                    NewAgent(AgentKind.Zombie, AgentMovement.Player, i);
                }
                else
                {
                    // Create AI zombie
                    NewAgent(AgentKind.Zombie, AgentMovement.AI, i);
                }
            }

            for (uint i = 0; i < options.Humans; i++)
            {

                if (i < options.PlayerHumans)
                {
                    // Create player-controlled human
                    NewAgent(AgentKind.Human, AgentMovement.Player,
                        options.Zombies + i);
                }
                else
                {
                    // Create AI human
                    NewAgent(AgentKind.Human, AgentMovement.AI,
                        options.Zombies + i);
                }
            }
        }

        private void NewAgent(AgentKind kind, AgentMovement movement, uint i)
        {
            int x, y;
            bool hasPlace = false;
            Agent agent;

            do
            {
                x = rand.Next((int)options.XDim);
                y = rand.Next((int)options.YDim);
                if (world[x, y] == null) hasPlace = true;
            } while (!hasPlace);

            agent = new Agent(x, y, kind, movement, world);
            agents[i] = agent;
        }

        public void Play()
        {
            // First render
            ui.RenderWorld(world);

            // Game loop
            for (int i = 0; i < options.Turns; i++)
            {
                // Shuffle agent list

                // Cycle through agents and make them play
                foreach (Agent agent in agents)
                {
                    agent.Play();
                }

                // Render
                ui.RenderWorld(world);
            }
        }
    }
}